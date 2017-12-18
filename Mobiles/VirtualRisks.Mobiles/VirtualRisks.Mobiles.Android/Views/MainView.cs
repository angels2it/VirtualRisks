using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
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
            map.UiSettings.ZoomControlsEnabled = true;
            ViewModel.GetCastlesAsync().ContinueWith(r =>
            {
                RunOnUiThread(() =>
                {
                    map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(ViewModel.Castles[0].Position.Lat, ViewModel.Castles[0].Position.Lng), 15));
                    foreach (var castle in ViewModel.Castles)
                    {
                        var option = new MarkerOptions();
                        option.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                        option.SetTitle("Castle " + castle.Index);
                        var latlng = new LatLng(castle.Position.Lat, castle.Position.Lng);
                        option.SetPosition(latlng);
                        map.AddMarker(option);
                    }
                    foreach (var route in ViewModel.Routes)
                    {
                        var polyline = new PolylineOptions();
                        polyline.Add(new LatLng(route.FromCastle.Position.Lat.Value, route.FromCastle.Position.Lng.Value));
                        polyline.Add(new LatLng(route.ToCastle.Position.Lat.Value, route.ToCastle.Position.Lng.Value));
                        polyline.InvokeColor(Color.Red);
                        map.AddPolyline(polyline);
                    }
                });
            });

        }
    }
}

