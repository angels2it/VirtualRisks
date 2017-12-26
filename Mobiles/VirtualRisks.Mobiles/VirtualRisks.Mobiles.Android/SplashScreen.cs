using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace VirtualRisks.Mobiles.Droid
{
    [Activity(
        Label = "VirtualRisks.Mobiles.Droid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
            UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
        }
    }
}
