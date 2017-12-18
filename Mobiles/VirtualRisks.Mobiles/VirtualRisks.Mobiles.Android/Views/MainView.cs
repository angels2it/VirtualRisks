using System.Collections.Generic;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using VirtualRisks.Mobiles.ViewModels;

namespace VirtualRisks.Mobiles.Droid.Views
{
   
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Splash")]
    public class MainView : MvxFragmentActivity<MainViewModel>, IOnMapReadyCallback
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }
        public void OnMapReady(GoogleMap map)
        {
            ViewModel.GetCastlesAsync().ContinueWith(r =>
            {
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.Castles[0].Position.Lat, ViewModel.Castles[0].Position.Lng), 15));
                foreach (var castle in ViewModel.Castles)
                {
                    var option = new MarkerOptions();
                    option.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                    option.SetPosition(new LatLng(castle.Position.Lat, castle.Position.Lng));
                    map.AddMarker(option);
                }
            });
            
        }
    }
}
