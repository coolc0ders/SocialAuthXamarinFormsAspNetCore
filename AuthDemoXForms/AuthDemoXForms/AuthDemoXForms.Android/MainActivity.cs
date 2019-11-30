using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AuthDemoXForms.Droid
{
    [Activity(Label = "AuthDemoXForms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            Xamarin.Auth.CustomTabsConfiguration.CustomTabsClosingMessage = null;
            Xamarin.Auth.CustomTabsConfiguration.IsActionButtonUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsActionBarToolbarIconUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsCloseButtonIconUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsShowTitleUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsDefaultShareMenuItemUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsPrefetchUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsUrlBarHidingUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.IsWarmUpUsed = false;
            Xamarin.Auth.CustomTabsConfiguration.MenuItemTitle = null;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}