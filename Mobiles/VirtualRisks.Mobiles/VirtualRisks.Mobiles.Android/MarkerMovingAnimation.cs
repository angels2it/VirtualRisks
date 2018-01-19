using System;
using System.Collections.Generic;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views.Animations;
using Java.Lang;
using Object = Java.Lang.Object;

namespace VirtualRisks.Mobiles.Droid
{
    public class MarkerMovingAnimation
    {
        private readonly GoogleMap _mMap;
        public MarkerMovingAnimation(GoogleMap mMap)
        {
            _mMap = mMap;
        }
        public void AnimateMarker(int position, LatLng startPosition, List<LatLng> wayPoints)
        {
            if (wayPoints == null || !wayPoints.Any())
                return;
            Marker marker = _mMap.AddMarker(new MarkerOptions()
                .SetPosition(startPosition)
                .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.marker_tank_red)));
            Handler handler = new Handler();
            long start = SystemClock.UptimeMillis();
            long duration = 1000;
            var interpolator = new LinearInterpolator();
            handler.Post(new MoveRunnable(handler, interpolator, marker, start, startPosition, wayPoints[position], duration,
                () =>
                {
                    position++;
                    if (wayPoints.Count > position)
                        AnimateMarker(position, startPosition, wayPoints);
                }));
        }
    }

    public class MoveRunnable : Object, IRunnable
    {
        private readonly Handler _handler;
        private readonly Marker _marker;
        private readonly LinearInterpolator _interpolator;
        private readonly long _start;
        private readonly LatLng _toPosition;
        private readonly LatLng _startPosition;
        private readonly long _duration;
        private readonly Action _endAction;
        public MoveRunnable(Handler handler, LinearInterpolator interpolator, Marker m, long start, LatLng startPosition, LatLng toPosition, long duration, Action endAction)
        {
            _handler = handler;
            _marker = m;
            _interpolator = interpolator;
            _start = start;
            _startPosition = startPosition;
            _toPosition = toPosition;
            _duration = duration;
            _endAction = endAction;
        }


        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - _start;
            float t = _interpolator.GetInterpolation((float)elapsed
                                                    / _duration);
            double lng = t * _toPosition.Longitude + (1 - t)
                         * _startPosition.Longitude;
            double lat = t * _toPosition.Latitude + (1 - t)
                         * _startPosition.Latitude;

            _marker.Position = new LatLng(lat, lng);

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