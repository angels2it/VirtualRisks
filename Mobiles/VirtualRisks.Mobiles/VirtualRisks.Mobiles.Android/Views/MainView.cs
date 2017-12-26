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
using Acr.UserDialogs;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using VirtualRisks.WebApi.RestClient.Models;
using MvvmCross.Binding.BindingContext;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class MainView : MvxFragmentActivity<MainViewModel>, IOnMapReadyCallback
    {
        private GoogleMap _map;
        private List<Polyline> _polylines = new List<Polyline>();
        private List<CastleRouteStateModel> _routes;
        private IMvxInteraction<GameStateUpdate> _interaction;
        public IMvxInteraction<GameStateUpdate> Interaction
        {
            get => _interaction;
            set
            {
                if (_interaction != null)
                    _interaction.Requested -= OnInteractionRequested;

                _interaction = value;
                _interaction.Requested += OnInteractionRequested;
            }
        }
        private void OnInteractionRequested(object sender, MvxValueEventArgs<GameStateUpdate> eventArgs)
        {
            _routes = eventArgs.Value.Routes;
            RunOnUiThread(() =>
            {
                _map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(eventArgs.Value.Castles[0].Position.Lat.GetValueOrDefault(0),
                    eventArgs.Value.Castles[0].Position.Lng.GetValueOrDefault(0)), 15));
                foreach (var castle in eventArgs.Value.Castles)
                {
                    var option = new MarkerOptions();
                    option.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                    option.SetTitle("Castle " + castle.Name);
                    option.SetSnippet(castle.Id);
                    var latlng = new LatLng(castle.Position.Lat.GetValueOrDefault(0), castle.Position.Lng.GetValueOrDefault(0));
                    option.SetPosition(latlng);
                    _map.AddMarker(option);
                }
            });
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.GameUpdate).OneWay();
            set.Apply();

            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }
        public void OnMapReady(GoogleMap map)
        {
            map.UiSettings.ZoomControlsEnabled = true;
            _map = map;
            _map.MarkerClick += _map_MarkerClick;
        }

        protected override void OnDestroy()
        {
            if (_map != null)
            {
                _map.MarkerClick -= _map_MarkerClick;
            }
            base.OnDestroy();
        }

        private void _map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            foreach (var polyline in _polylines)
            {
                polyline.Remove();
            }
            _polylines.Clear();

            var routes = _routes.Where(r => r.FromCastle == e.Marker.Snippet || r.ToCastle == e.Marker.Snippet);
            if(routes == null)
                return;
            foreach (var route in routes)
            {
                if (route?.Route?.Steps.Count == 0)
                    continue;
                var polyline = new PolylineOptions();
                foreach (var step in route.Route.Steps)
                {
                    polyline.Add(new LatLng(step.StartLocation.Lat.Value, step.StartLocation.Lng.Value));
                    polyline.Add(new LatLng(step.EndLocation.Lat.Value, step.EndLocation.Lng.Value));
                }
                polyline.InvokeColor(Color.Red);
                _polylines.Add(_map.AddPolyline(polyline));
            }
        }
    }
}

