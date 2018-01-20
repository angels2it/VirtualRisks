using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views.Animations;
using Java.Lang;
using Rangstrup.Xam.Plugin.Maps.Droid;
using Object = Java.Lang.Object;

namespace VirtualRisks.Mobiles.Droid
{
    public class MoveRunnable : Object, IRunnable
    {
        private readonly GoogleMap _gMap;
        private readonly Handler _handler;
        private readonly Marker _marker;
        private readonly LinearInterpolator _interpolator;
        private readonly long _start;
        private readonly LatLng _toPosition;
        private readonly LatLng _startPosition;
        private readonly double _duration;
        private readonly Action _endAction;
        public MoveRunnable(GoogleMap gMap, Handler handler, LinearInterpolator interpolator, Marker m, long start, LatLng startPosition, LatLng toPosition, double duration, Action endAction)
        {
            _gMap = gMap;
            _handler = handler;
            _marker = m;
            _interpolator = interpolator;
            _start = start;
            _startPosition = startPosition;
            _toPosition = toPosition;
            _duration = duration;
            _endAction = endAction;
        }


        private bool NeedUpdateRotation()
        {
            return true;
            if (_marker == null)
                return false;
            if (_lastUpdatedLocation == null)
                return true;
            if (MapHelpers.GetDistance(_marker.Position.Latitude, _marker.Position.Longitude, _lastUpdatedLocation.Latitude, _lastUpdatedLocation.Longitude) * 1000 >
                3)
                return true;
            return false;
        }
        private LatLng _lastUpdatedLocation = null;
        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - _start;
            float t = _interpolator.GetInterpolation((float)elapsed
                                                     / (float)_duration);
            double lng = t * _toPosition.Longitude + (1 - t)
                         * _startPosition.Longitude;
            double lat = t * _toPosition.Latitude + (1 - t)
                         * _startPosition.Latitude;

            var endLatLng = new LatLng(lat, lng);
            if (NeedUpdateRotation())
            {
                var heading = SphericalUtil.computeHeading(_marker.Position, endLatLng);
                heading = heading + 180;
                //if (Options.HeadingConverter != null)
                //    heading = Options.HeadingConverter(heading);
                double mapHeading = 0;
                if (_gMap?.CameraPosition != null)
                {
                    mapHeading = -_gMap.CameraPosition.Bearing;
                }
                _marker.Rotation = (float)heading + (float)mapHeading;
                _lastUpdatedLocation = endLatLng;
            }

            _marker.Position = endLatLng;

            if (t < 1.0)
            {
                // Post again 16ms later.
                _handler.PostDelayed(this, 16);
            }
            else
            {
                _endAction?.Invoke();
            }
            //else
            //{
            //    if (hideMarker)
            //    {
            //        marker.Visible = false;
            //    }
            //    else
            //    {
            //        marker.Visible = true;
            //    }
            //}
        }
    }
}