using System;
using System.Linq;
using Android.Animation;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views.Animations;
using Java.Interop;
using Rangstrup.Xam.Plugin.Maps.Droid;
using VirtualRisks.WebApi.RestClient.Models;
using Object = Java.Lang.Object;

namespace VirtualRisks.Mobiles.Droid
{
    public abstract class BaseMapAnimator : Object
    {
        string Index { get; set; }
        public abstract void Start(GoogleMap map, RouteModel route);
        public abstract void Stop();
    }
    public class MapMarkerMovementAnimator : BaseMapAnimator
    {
        private readonly Marker _marker;
        private AnimatorSet _firstRunAnimSet;
        private GoogleMap _googleMap;
        public string Index { get; set; }
        public MapMarkerMovementAnimator(string index, Marker marker)
        {
            Index = index;
            _marker = marker;
        }
        public override void Start(GoogleMap googleMap, RouteModel route)
        {
            _googleMap = googleMap;
            if (_firstRunAnimSet == null)
            {
                _firstRunAnimSet = new AnimatorSet();
            }
            else
            {
                _firstRunAnimSet.RemoveAllListeners();
                _firstRunAnimSet.End();
                _firstRunAnimSet.Cancel();

                _firstRunAnimSet = new AnimatorSet();
            }

            var currentStep = route.Steps[0];
            LatLng[] bangaloreRoute;
            if (currentStep.StartLocation.Lat != _marker.Position.Latitude)
            {
                bangaloreRoute = new LatLng[]
                {
                    new LatLng(currentStep.EndLocation.Lat.Value,currentStep.EndLocation.Lng.Value),
                    new LatLng(currentStep.StartLocation.Lat.Value,currentStep.StartLocation.Lng.Value)
                };
            }
            else
            {
                bangaloreRoute = new LatLng[]
                {
                    new LatLng(currentStep.StartLocation.Lat.Value,currentStep.StartLocation.Lng.Value),
                    new LatLng(currentStep.EndLocation.Lat.Value,currentStep.EndLocation.Lng.Value)
                };
            }
            
            if (bangaloreRoute.Length == 0)
                return;
            var foregroundRouteAnimator = ObjectAnimator.OfObject(this, "routeIncreaseForward", new RouteEvaluator(), bangaloreRoute.ToArray<Object>());
            foregroundRouteAnimator.SetInterpolator(new AccelerateDecelerateInterpolator());
            foregroundRouteAnimator.SetDuration((long)TimeSpan.FromMinutes(3).TotalMilliseconds);
            foregroundRouteAnimator.RepeatCount = ValueAnimator.Infinite;
            _firstRunAnimSet.PlaySequentially(foregroundRouteAnimator);
            _firstRunAnimSet.Start();
        }


        /**
         * This will be invoked by the ObjectAnimator multiple times. Mostly every 16ms.
         **/
        private LatLng _lastUpdatedLocation = null;
        [Export]
        public void setRouteIncreaseForward(LatLng endLatLng)
        {
            if (endLatLng == null)
                return;
            if (_marker != null)
            {
                if (NeedUpdateRotation())
                {
                    var heading = SphericalUtil.computeHeading(_marker.Position, endLatLng);
                    heading = heading + 180;
                    //if (Options.HeadingConverter != null)
                    //    heading = Options.HeadingConverter(heading);
                    double mapHeading = 0;
                    if (_googleMap?.CameraPosition != null)
                    {
                        mapHeading = -_googleMap.CameraPosition.Bearing;
                    }
                    _marker.Rotation = (float)heading + (float)mapHeading;
                    _lastUpdatedLocation = endLatLng;
                }
                _marker.Position = endLatLng;
            }
        }

        private bool NeedUpdateRotation()
        {
            if (_marker == null)
                return false;
            if (_lastUpdatedLocation == null)
                return true;
            if (MapHelpers.GetDistance(_marker.Position.Latitude, _marker.Position.Longitude, _lastUpdatedLocation.Latitude, _lastUpdatedLocation.Longitude) * 1000 >
                3)
                return true;
            return false;
        }

        public override void Stop()
        {
            if (_firstRunAnimSet == null)
                return;
            _firstRunAnimSet.RemoveAllListeners();
            _firstRunAnimSet.End();
            _firstRunAnimSet.Cancel();
        }
    }
}