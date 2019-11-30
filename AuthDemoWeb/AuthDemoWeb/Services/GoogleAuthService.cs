using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthDemoWeb.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace AuthDemoWeb.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;

        public GoogleAuthService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> Authenticate(UserTokenIdModel userTokenModel)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(userTokenModel.TokenId, new GoogleJsonWebSignature.ValidationSettings());
            return await CreateOrGetUser(payload, userTokenModel);
        }

        private async Task<AppUser> CreateOrGetUser(Payload payload, UserTokenIdModel userToken)
        {
            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    FirstName = userToken.Name,
                    SecondName = userToken.FamilyName,
                    Email = userToken.Email,
                    PictureURL = userToken.Picture,
                    OAuthIssuer = payload.Issuer,
                    OAuthSubject = payload.Subject,
                    UserName = userToken.Name.Replace(" ", "_")
                };
                var identityUser = await _userManager.CreateAsync(appUser);
                return appUser;
            }

            return user;
        }
    }
}
