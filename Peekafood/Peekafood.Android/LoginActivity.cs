using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using Android.Support.V7.App;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;

namespace Peekafood.Droid
{
    [Activity(Label = "LoginActivity", Icon = "@drawable/icon", Theme = "@style/Theme.MyTheme")]
    public class LoginActivity : AppCompatActivity, IFacebookCallback
    {
        private ICallbackManager mCallBackManager;

        public void OnCancel()
        {
            Toast.MakeText(this.ApplicationContext, "Cancelled", ToastLength.Short).Show();
        }

        public void OnError(FacebookException error)
        {
            Toast.MakeText(this.ApplicationContext, "Error", ToastLength.Short).Show();
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);

            if (IsLoggedIn())
            {
                Intent i = new Intent(this, typeof(MainActivity));
                StartActivity(i);

            }
            else
            {

                LoginButton loginButton = FindViewById<LoginButton>(Resource.Id.login_button);

                loginButton.SetReadPermissions("user_friends", "public_profile");
                mCallBackManager = CallbackManagerFactory.Create();

                loginButton.RegisterCallback(mCallBackManager, this);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        public static bool IsLoggedIn()
        {
            AccessToken accessToken = AccessToken.CurrentAccessToken;
            return accessToken != null;
        }

        public override void OnBackPressed()
        {
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            startMain.SetFlags(ActivityFlags.NewTask);
            StartActivity(startMain);
            return;
        }
    }
}