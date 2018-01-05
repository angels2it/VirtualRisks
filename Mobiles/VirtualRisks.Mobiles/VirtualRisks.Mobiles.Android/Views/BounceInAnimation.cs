using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views.Animations;
using Java.Lang;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [Register("virtualrisks.mobiles.droid.views.BounceInAnimation")]
    public class BounceInAnimation : Java.Lang.Object, IRunnable
    {
        private long mStart, mDuration;
        private IInterpolator mInterpolator;
        private Marker mMarker;
        private Handler mHandler;
        private LatLng _startLatLng;
        private LatLng _target;

        public BounceInAnimation(Marker marker, LatLng startLatLng, LatLng target)
        {
            mMarker = marker;
            mHandler = new Handler();
            mInterpolator = new LinearInterpolator();
            mStart = SystemClock.UptimeMillis();
            mDuration = 1000;
            _startLatLng = startLatLng;
            _target = target;
        }

        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - mStart;
            float t = mInterpolator.GetInterpolation((float)elapsed / mDuration);
            double lng = t * _target.Longitude + (1 - t) * _startLatLng.Longitude;
            double lat = t * _target.Latitude + (1 - t) * _startLatLng.Latitude;
            mMarker.Position = new LatLng(lat, lng);
            if (t < 1.0)
            {
                // Post again 10ms later.
                mHandler.PostDelayed(this, 10);
            }
            else
            {
                // animation ended
            }
        }
    }
}