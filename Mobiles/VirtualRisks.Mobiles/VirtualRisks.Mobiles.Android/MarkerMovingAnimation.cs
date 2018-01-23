using System.Collections.Generic;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views.Animations;

namespace VirtualRisks.Mobiles.Droid
{
    public class MarkerMovingAnimation
    {
        private Marker _mMarker;
        private GoogleMap _mMap;
        private double totalDuration;
        private double totalDistance;
        private bool isInit;
        public MarkerMovingAnimation(GoogleMap mMap, Marker marker)
        {
            _mMap = mMap;
            _mMarker = marker;
        }
        public void AnimateMarker(double duration, LatLng startPosition, List<LatLng> wayPoints, int position)
        {
            if (wayPoints == null || !wayPoints.Any() || duration == 0)
                return;
            if (!isInit)
            {
                totalDuration = duration;
                totalDistance = CalculatorDistance(startPosition, wayPoints);
                isInit = true;
            }
            Handler handler = new Handler();
            long start = SystemClock.UptimeMillis();
            var interpolator = new LinearInterpolator();
            var nextLocation = wayPoints[position];
            var nextDuration = MapHelpers.GetDistance(startPosition.Latitude, startPosition.Longitude,
                nextLocation.Latitude, nextLocation.Longitude) *1.0 / totalDistance * totalDuration;
            handler.Post(new MoveRunnable(_mMap, handler, interpolator, _mMarker, start, startPosition, nextLocation, nextDuration,
                () =>
                {
                    position++;
                    if (wayPoints.Count > position)
                        AnimateMarker(duration, _mMarker.Position, wayPoints, position);
                    else
                        _mMarker.Remove();
                }));
        }

        private double CalculatorDistance(LatLng start, List<LatLng> wayPoints)
        {
            if (!(wayPoints?.Any() ?? false))
                return 0;

            double distance = 0;
            var startLocation = start;
            for (int i = 0; i < wayPoints.Count; i++)
            {
                var nextLocation = wayPoints[i];
                distance += MapHelpers.GetDistance(startLocation.Latitude, startLocation.Longitude,
                    nextLocation.Latitude, nextLocation.Longitude);
                startLocation = nextLocation;
            }

            return distance;
        }
    }
}