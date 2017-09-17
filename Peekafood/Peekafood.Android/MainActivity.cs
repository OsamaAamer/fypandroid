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
using Org.Json;
using Android.Views;

namespace Peekafood.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, Icon = "@drawable/icon", Theme = "@style/Theme.MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        BottomNavigationView bottomNavigation;
        public static System.String facebookUserName;

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

                Bundle bundle = new Bundle();
                bundle.PutString("fields", "name, id");

                GraphCallback graphCallBack = new GraphCallback();
                graphCallBack.RequestCompleted += OnGetFriendsResponse;
                var request = new GraphRequest(accessToken, "/" + accessToken.UserId, bundle, HttpMethod.Get, graphCallBack).ExecuteAsync();                
            }
        }

        public void OnGetFriendsResponse(object sender, GraphResponseEventArgs e)
        {
            JSONObject UserObject = e.Response.JSONObject;

            System.String fullName = UserObject.OptString("name");
            System.String id = UserObject.OptString("id");

            facebookUserName = fullName;

            return;
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            System.String fragmentTag = "";

            switch (id)
            {
                case Resource.Id.menu_home:
                    fragment = Fragment1.NewInstance();
                    fragmentTag = "FRAGMENT1";
                    break;
                case Resource.Id.menu_audio:
                    fragment = Fragment2.NewInstance();
                    fragmentTag = "FRAGMENT2";
                    break;
                case Resource.Id.menu_video:
                    fragment = Fragment3.NewInstance();
                    fragmentTag = "FRAGMENT3";
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment, fragmentTag)
               .Commit();
        }

        public override void OnBackPressed()
        {
            Android.Support.V4.App.Fragment myFragment = null;
            myFragment = SupportFragmentManager.FindFragmentByTag("FRAGMENT1");

            if (LoginActivity.IsLoggedIn())
            {
                if (myFragment != null && myFragment.IsVisible)
                {
                    MinimizeApp();
                }
                else
                {
                    LoadFragment(Resource.Id.menu_home);
                }
                return;
            }

            base.OnBackPressed();
        }

        private void MinimizeApp()
        {
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            startMain.SetFlags(ActivityFlags.NewTask);
            StartActivity(startMain);
        }
    }

    class GraphCallback : Java.Lang.Object, GraphRequest.ICallback
    {
        // Event to pass the response when it's completed
        public event EventHandler<GraphResponseEventArgs> RequestCompleted = delegate { };

        public void OnCompleted(GraphResponse reponse)
        {
            this.RequestCompleted(this, new GraphResponseEventArgs(reponse));
        }
    }

    public class GraphResponseEventArgs : EventArgs
    {
        GraphResponse _response;
        public GraphResponseEventArgs(GraphResponse response)
        {
            _response = response;
        }

        public GraphResponse Response { get { return _response; } }
    }
}