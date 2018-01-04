using System;
using Android.Animation;
using Android.Gms.Maps.Model;
using Object = Java.Lang.Object;

namespace VirtualRisks.Mobiles.Droid
{
    public class RouteEvaluator : Object, ITypeEvaluator
    {
        public Object Evaluate(float fraction, Object startValue, Object endValue)
        {
            try
            {
                var startPoint = (LatLng)startValue;
                var endPoint = (LatLng)endValue;
                double lat = startPoint.Latitude + fraction * (endPoint.Latitude - startPoint.Latitude);
                double lng = startPoint.Longitude + fraction * (endPoint.Longitude - startPoint.Longitude);
                return new LatLng(lat, lng);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}