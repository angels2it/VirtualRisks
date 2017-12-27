using Android.App;
using Android.Content;
using Xamarin.Facebook;

namespace VirtualRisks.Mobiles.Droid
{
    public static class SocialLoginDroid
    {
        public static void Init(Activity activity)
        {
            FacebookSdk.SdkInitialize(activity);
            CallbackManager = CallbackManagerFactory.Create();
        }
        public static ICallbackManager CallbackManager;

        public static void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}