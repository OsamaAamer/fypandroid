using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace Peekafood.Droid.Fragments
{
    public class Fragment3 : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Fragment3 NewInstance()
        {
            var frag3 = new Fragment3 { Arguments = new Bundle() };
            return frag3;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View inflatedView = inflater.Inflate(Resource.Layout.fragment3, null);

            TextView mTextName = (TextView)inflatedView.FindViewById(Resource.Id.textName);
            mTextName.Text = MainActivity.facebookUserName;

            return inflatedView;
        }
    }
}