using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views.Animations;
using Java.Lang;

namespace VirtualRisks.Mobiles.Droid.Animations
{
    [Register("virtualrisks.mobiles.droid.views.animations.BounceInAnimation")]
    public class BounceInAnimation : Java.Lang.Object, IRunnable
    {
        private readonly long _mStart;
        private readonly long _mDuration;
        private readonly IInterpolator _mInterpolator;
        private readonly Marker _mMarker;
        private readonly Handler _mHandler;
        private readonly LatLng _startLatLng;
        private readonly LatLng _target;

        public BounceInAnimation(Marker marker, LatLng startLatLng, LatLng target)
        {
            _mMarker = marker;
            _mHandler = new Handler();
            _mInterpolator = new LinearInterpolator();
            _mStart = SystemClock.UptimeMillis();
            _mDuration = 1000;
            _startLatLng = startLatLng;
            _target = target;
        }

        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - _mStart;
            float t = _mInterpolator.GetInterpolation((float)elapsed / _mDuration);
            double lng = t * _target.Longitude + (1 - t) * _startLatLng.Longitude;
            double lat = t * _target.Latitude + (1 - t) * _startLatLng.Latitude;
            _mMarker.Position = new LatLng(lat, lng);
            if (t < 1.0)
            {
                // Post again 10ms later.
                _mHandler.PostDelayed(this, 10);
            }
            else
            {
                // animation ended
                _mMarker.Position = _target;
            }
        }
    }
}