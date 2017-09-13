using Android.App;
using Android.OS;
using Peekafood.Droid.Fragments;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Java.Security;
using System;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;
using Java.Lang;
using Android.Content;
using Android.Runtime;
using Xamarin.Facebook.Login;
using Android.Widget;

namespace Peekafood.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon", Theme = "@style/Theme.MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            //FacebookSdk.SdkInitialize(ApplicationContext);

            SetContentView(Resource.Layout.main);

            CheckLoginStatus();

            // Code to generate key hash
            /*PackageInfo info = this.PackageManager.GetPackageInfo("com.peekafood", PackageInfoFlags.Signatures);

            foreach (Android.Content.PM.Signature signature in info.Signatures) {
                MessageDigest md = MessageDigest.GetInstance("SHA");
                md.Update(signature.ToByteArray());

                string keyhash = Convert.ToBase64String(md.Digest());
                Console.WriteLine("KeyHash", keyhash);
            }*/

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);

            }

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.menu_home);

            FindViewById<Button>(Resource.Id.logoutButton).Click += (o, e) =>
            {
                LoginManager.Instance.LogOut();

                Intent i = new Intent(this, typeof(LoginActivity));
                StartActivity(i);
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            CheckLoginStatus();
        }

        protected override void OnResume()
        {
            base.OnResume();
            CheckLoginStatus();
        }

        private void CheckLoginStatus()
        {
            if (!LoginActivity.IsLoggedIn())
            {
                Toast.MakeText(this.ApplicationContext, "Please login to continue", ToastLength.Short).Show();

                Intent i = new Intent(this, typeof(LoginActivity));
                StartActivity(i);
            }
            else
            {
                AccessToken accessToken = AccessToken.CurrentAccessToken;
                //loadUserDetails(accessToken);
            }
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_home:
                    fragment = Fragment1.NewInstance();
                    break;
                case Resource.Id.menu_audio:
                    fragment = Fragment2.NewInstance();
                    break;
                case Resource.Id.menu_video:
                    fragment = Fragment3.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
    }
}