using AuthDemoXForms.Models;
using AuthDemoXForms.Models.Facebook;
using AuthDemoXForms.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace AuthDemoXForms.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        public Command GoogleAuthCommand { get; set; }
        const string ClientId = "...-...apps.googleusercontent.com";
        OAuth2Authenticator _authenticator;
        public Command FacebookAuthCommand { get; set; }

        public AuthViewModel()
        {
            GoogleAuthCommand = new Command(async () => await GoogleAuth());
            FacebookAuthCommand = new Command(async () => await FacebookAuth());
        }

        private async Task GoogleAuth()
        {
            _authenticator = new OAuth2Authenticator(
                clientId: ClientId, 
                clientSecret: "",
                scope: "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile",
                authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                redirectUrl: new Uri("com.googleusercontent.apps....-...:/oauth2redirect"),
                accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                isUsingNativeUI: true
                );

            _authenticator.Completed += Authenticator_Completed;
            _authenticator.Error += _authenticator_Error;
            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }

        async Task FacebookAuth()
        {
            var facebookAppId = "";
            _authenticator = new OAuth2Authenticator(
                clientId: facebookAppId,
                scope: "email",
                authorizeUrl: new Uri("https://www.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
                );
            _authenticator.Completed += Authenticator_Facebook_Completed;
            _authenticator.Error += _authenticator_Error;
            AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }

        private void _authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticator.Error -= _authenticator_Error;
            Debug.WriteLine(e.Message);
        }

        private async void Authenticator_Facebook_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            _authenticator.Completed -= Authenticator_Facebook_Completed;
            if (e.IsAuthenticated)
            {
                var acc = e.Account;
                var val = JsonConvert.SerializeObject(e.Account);
                var model = JsonConvert.DeserializeObject<FacebookSigninModel>(val);
                var user = await GetFacebookProfile(model.Properties.AccessToken);
                await SignInToAPI(new APIUser
                {
                    TokenId = model.Properties.AccessToken,
                    Email = user.Email, 
                    FamilyName = user.FirstName,
                    GivenName = user.LastName,
                    Picture = user.ProfilePic
                }, AuthProvider.Facebook);
            }
        }

        public async Task<FacebookUser> GetFacebookProfile(string accessToken)
        {
            using var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(
                $"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={accessToken}");
            var profilePic = await GetFacebookProfilePicURL(accessToken, "");

            var user = JsonConvert.DeserializeObject<FacebookUser>(json);
            user.ProfilePic = profilePic;

            return user;
        }

        public async Task<string> GetFacebookProfilePicURL(string accessToken, string userId)
        {
            using var httpClient = new HttpClient();
            var picUrl = $"https://graph.facebook.com/v5.0/me/picture?redirect=false&type=normal&access_token={accessToken}";
            var res = await httpClient.GetStringAsync(picUrl);
            var pic = JsonConvert.DeserializeAnonymousType(res, new { data = new PictureData() });
            return pic.data.Url;
        }


        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            _authenticator.Completed -= Authenticator_Completed;
            if (e.IsAuthenticated)
            {
                var acc = e.Account;
                var usr = await GetGoogleUser(acc.Properties["id_token"], e.Account);
                await SignInToAPI(usr, AuthProvider.Google);
            }
        }

        private async Task<APIUser> GetGoogleUser(string idToken, Account account)
        {
            //Get user's profile info from Google
            var request = new OAuth2Request("GET", new Uri("https://www.googleapis.com/oauth2/v2/userinfo"), null, account);
            var response = await request.GetResponseAsync();
            APIUser user = null;

            if (response != null)
            {
                string userJson = await response.GetResponseTextAsync();
                user = JsonConvert.DeserializeObject<APIUser>(userJson);
                user.TokenId = idToken;
                return user;
            }
            throw new Exception("Could not get user");
        }

        private async Task SignInToAPI(APIUser user, AuthProvider authProvider)
        {
            //Used Ngrok to tunnel My endpoint.
            var endPoint = "https://5d13182b.ngrok.io";
            if (authProvider == AuthProvider.Google)
                endPoint = $"{endPoint}/Account/Google";
            else
                endPoint = $"{endPoint}/Account/Facebook";

            using var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(endPoint, content);

            //We get the JWT Token which we will be used to make authorized request to the API.
            var jwt = await res.Content.ReadAsStringAsync();
        }
    }
    public enum AuthProvider
    {
        Google,
        Facebook
    }
    public class AuthenticationState
    {
        public static OAuth2Authenticator Authenticator;
    }
}
