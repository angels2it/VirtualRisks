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
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class MainView : MvxFragmentActivity<MainViewModel>, IOnMapReadyCallback
    {
        private MvxFluentBindingDescriptionSet<MainView, MainViewModel> _set;
        private GoogleMap _map;
        private ProgressBar _pbLoading;
        private List<Polyline> _polylines = new List<Polyline>();
        private List<CastleRouteStateModel> _routes;
        private List<CastleStateModel> _castles;
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
            _castles = eventArgs.Value.Castles;
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
                    option.Draggable(true);
                    var latlng = new LatLng(castle.Position.Lat.GetValueOrDefault(0), castle.Position.Lng.GetValueOrDefault(0));
                    option.SetPosition(latlng);
                    _map.AddMarker(option);
                }
            });
        }

        private IMvxInteraction<bool> _loading;
        public IMvxInteraction<bool> Loading
        {
            get => _loading;
            set
            {
                if (_loading != null)
                    _loading.Requested -= OnLoadingRequested;

                _loading = value;
                _loading.Requested += OnLoadingRequested;
            }
        }

        private void OnLoadingRequested(object sender, MvxValueEventArgs<bool> eventArgs)
        {
            RunOnUiThread(() =>
            {
                _pbLoading.Visibility = eventArgs.Value ? ViewStates.Visible : ViewStates.Gone;
            });
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            SocialLoginDroid.Init(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            _set = this.CreateBindingSet<MainView, MainViewModel>();
            _set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.GameUpdate).OneWay();
            SetLoadingControl();
            _set.Apply();
            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }

        private void SetLoadingControl()
        {
            _pbLoading = FindViewById<ProgressBar>(Resource.Id.pbLoading);
            _pbLoading.Indeterminate = true;
            _pbLoading.IndeterminateTintList = ColorStateList.ValueOf(Color.Red);

            _set.Bind(this)
                .For(e => e.Loading)
                .To(vm => vm.Loading)
                .OneWay();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            SocialLoginDroid.OnActivityResult(requestCode, resultCode, data);
        }
        public void OnMapReady(GoogleMap map)
        {
            map.UiSettings.ZoomControlsEnabled = false;
            _map = map;
            _map.MarkerClick += _map_MarkerClick;
            _map.MarkerDragEnd += _map_MarkerDragEnd;
        }

        private void _map_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            var fromCastle = _castles.FirstOrDefault(c => c.Id == e.Marker.Snippet);
            if (fromCastle == null)
                return;
            var dragAbleCastles = _routes.Where(r => r.FromCastle == e.Marker.Snippet || r.ToCastle == e.Marker.Snippet)
                .SelectMany(r => new[] { r.FromCastle, r.ToCastle }).Distinct().Except(new[] { e.Marker.Snippet }).ToList();
            var nearestCastle = _castles.Where(c => dragAbleCastles.Contains(c.Id)).Select(c => new
            {
                Castle = c,
                Distance = MapHelpers.GetDistance(c.Position.Lat.Value, c.Position.Lng.Value, e.Marker.Position.Latitude,
                            e.Marker.Position.Longitude)
            }).OrderBy(d => d.Distance).First();

            if (nearestCastle.Distance * 1000 > 50)
                ViewModel.DragCastleLargeDistance();
            else
                ViewModel.DragToCastle(e.Marker.Snippet, nearestCastle.Castle.Id);
            e.Marker.Position = new LatLng(fromCastle.Position.Lat.Value, fromCastle.Position.Lng.Value);
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
            if (routes == null)
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

