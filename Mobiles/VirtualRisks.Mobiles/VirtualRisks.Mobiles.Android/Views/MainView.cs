using System.Collections.Generic;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;

namespace VirtualRisks.Mobiles.Droid.Views
{
    public class LocationModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public LocationModel(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }
    }
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Splash")]
    public class MainView : MvxFragmentActivity, IOnMapReadyCallback
    {
        List<LocationModel> _locations = new List<LocationModel>()
        {
            new LocationModel(10.78761,106.6987),
            new LocationModel(10.78739,106.69848),
            new LocationModel(10.78698,106.69809),
            new LocationModel(10.78646,106.69762),
            new LocationModel(10.78646,106.69762),
            new LocationModel(10.78551,106.69867),
            new LocationModel(10.78487,106.69932),
            new LocationModel(10.78454,106.69966),
            new LocationModel(10.78454,106.69966),
            new LocationModel(10.78506,106.70013),
            new LocationModel(10.78509,106.70016),
            new LocationModel(10.78528,106.70034),
            new LocationModel(10.78544 ,106.7005),
            new LocationModel(10.78581,106.70084),
            new LocationModel(10.78604,106.70109),
            new LocationModel(10.78644,106.70149),
            new LocationModel(10.78644,106.70149),
            new LocationModel(10.78654,106.70162),
            new LocationModel(10.78659,106.70157),
            new LocationModel(10.78784,106.70023)
        };
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }
        public void OnMapReady(GoogleMap map)
        {
            map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(_locations[0].Lat, _locations[0].Lng), 15));
            foreach (var location in _locations)
            {
                var option = new MarkerOptions();
                option.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                option.SetPosition(new LatLng(location.Lat, location.Lng));
                map.AddMarker(option);
            }
        }
    }
}
