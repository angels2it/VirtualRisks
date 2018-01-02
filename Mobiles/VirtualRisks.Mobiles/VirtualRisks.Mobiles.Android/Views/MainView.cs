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
using System.Threading.Tasks;
using Java.Net;
using Java.IO;
using System;
using Android.Runtime;
using Android.Views.Animations;
using Java.Lang;
using static Android.Resource;
using Java.Interop;
using Cheesebaron.SlidingUpPanel;
using Android.Support.V4.Widget;
using Android.Util;

namespace VirtualRisks.Mobiles.Droid.Views
{

    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class MainView : MvxFragmentActivity<MainViewModel>, IOnMapReadyCallback
    {
        private MvxFluentBindingDescriptionSet<MainView, MainViewModel> _set;
        private readonly Dictionary<string, Marker> _markerInstanceList = new Dictionary<string, Marker>();

        private Handler mHandler;
        private IRunnable mAnimation;

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

                    option.SetTitle("Castle " + castle.Name);
                    option.SetSnippet(castle.Id);
                    option.Draggable(true);
                    var latlng = new LatLng(castle.Position.Lat.GetValueOrDefault(0), castle.Position.Lng.GetValueOrDefault(0));
                    option.SetPosition(latlng);
                    var marker = _map.AddMarker(option);
                    if (!_markerInstanceList.ContainsKey(castle.Id))
                        _markerInstanceList.Add(marker.Id, marker);
                    SetupMarkerIcon(marker, GetIcon(castle));
                }
            });
        }

        private string GetIcon(CastleStateModel castle)
        {
            return castle.Army == "Red" ? "red_castle" : "blue_castle";
        }
        private void SetDefaultIcon(Marker marker)
        {
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
        }
        private bool IsUriIcon(string icon)
        {
            return !string.IsNullOrEmpty(icon) && (icon.StartsWith("http") || icon.StartsWith("https"));
        }
        private Dictionary<string, BitmapDescriptor> _cachedIcon = new Dictionary<string, BitmapDescriptor>();
        private void SetupMarkerIcon(Marker marker, string icon, bool forceUpdate = false)
        {
            if (marker == null)
                return;
            if (string.IsNullOrEmpty(icon))
            {
                SetDefaultIcon(marker);
                return;
            }
            if (IsUriIcon(icon))
            {
                if (_cachedIcon.ContainsKey(icon))
                {
                    marker.SetIcon(_cachedIcon[icon]);
                    return;
                }
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        URL url = new URL(icon);
                        var conn = (HttpURLConnection)url.OpenConnection();
                        conn.InstanceFollowRedirects = true;
                        Bitmap image = BitmapFactory.DecodeStream(conn.InputStream);
                        RunOnUiThread(() =>
                        {
                            _cachedIcon[icon] = BitmapDescriptorFactory.FromBitmap(image);
                            marker.SetIcon(_cachedIcon[icon]);
                        });
                    }
                    catch (IOException ex)
                    {

                    }
                });
                return;
            }
            try
            {
                var iconId = Resources.GetIdentifier(icon, "drawable", this.PackageName);
                if (iconId == 0)
                {
                    marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                }
                else
                {
                    var bitmap = GetBitmapByZoomLevel(icon, _map.CameraPosition.Zoom);
                    if (bitmap != null)
                        marker.SetIcon(bitmap);
                }
            }
            catch (System.Exception ex)
            {
                marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
            }
        }
        private int defaultZoomLevel = 15;
        private int maxIconSize = 64;
        private BitmapDescriptor GetBitmapByZoomLevel(string icon, float zoomLevel)
        {
            if (string.IsNullOrEmpty(icon))
                return BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed);
            var zoom = (int)System.Math.Ceiling(zoomLevel);
            var zoomPercent = zoom * 1.0 / defaultZoomLevel;

            var width = zoomPercent * (maxIconSize);
            if (width == 0)
                return BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed);
            var iconKey = $"{icon}_{zoom}";
            if (_cachedIcon.ContainsKey(iconKey))
            {
                return _cachedIcon[iconKey];
            }
            var file =
                BitmapDescriptorFactory.FromBitmap(
                    ResizeMapIcons(icon, (int)width));
            _cachedIcon[iconKey] = file;
            return file;
        }
        private Bitmap ResizeMapIcons(string iconName, int width)
        {
            try
            {
                var iconId = Resources.GetIdentifier(iconName, "drawable", PackageName);
                if (iconId <= 0) return null;
                Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, iconId);
                if (width > imageBitmap.Width)
                    width = imageBitmap.Width;
                var height = imageBitmap.Height * (width * 1.0 / imageBitmap.Width);
                Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(imageBitmap, width, (int)height, false);
                return resizedBitmap;
            }
            catch (System.Exception e)
            {
                return null;
            }
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
            mHandler = new Handler();
            SetContentView(Resource.Layout.MainView);
            SocialLoginDroid.Init(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            _set = this.CreateBindingSet<MainView, MainViewModel>();
            _set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.GameUpdate).OneWay();
            SetLoadingControl();
            SetBottomSheet();
            _set.Apply();
            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }

        private SlidingUpPanelLayout _bottom;
        public void SetBottomSheet()
        {
            _bottom = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);

            _bottom.ShadowDrawable = Resources.GetDrawable(Resource.Drawable.above_shadow);
            _bottom.AnchorPoint = 0.3f;
            _bottom.PanelHeight = 100;
            _bottom.PanelCollapsed += _bottom_PanelCollapsed;
            _bottom.PanelExpanded += _bottom_PanelExpanded            ;
            var slider = _bottom.GetChildAt(1);
            slider.Visibility = ViewStates.Gone;


            var viewDetail = SupportFragmentManager.FindFragmentById(Resource.Id.fragDetail) as CastleView;

        }

        private void _bottom_PanelExpanded(object sender, SlidingUpPanelEventArgs args)
        {
            var viewDetail = SupportFragmentManager.FindFragmentById(Resource.Id.fragDetail) as CastleView;
            viewDetail?.Init(ViewModel.SelectedCastle.Id);
        }

        private void _bottom_PanelCollapsed(object sender, SlidingUpPanelEventArgs args)
        {
            var slider = _bottom.GetChildAt(1);
            slider.Visibility = ViewStates.Gone;
        }

        private void SetLoadingControl()
        {
            _pbLoading = FindViewById<ProgressBar>(Resource.Id.pbLoading);
            _pbLoading.Indeterminate = true;
            _pbLoading.IndeterminateTintList = ColorStateList.ValueOf(Android.Graphics.Color.Red);

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
            _map.MarkerDragStart += _map_MarkerDragStart;
            _map.MarkerDragEnd += _map_MarkerDragEnd;
            _map.CameraChange += _map_CameraChange;
        }

        private Marker _tempMarker;
        private void _map_MarkerDragStart(object sender, GoogleMap.MarkerDragStartEventArgs e)
        {
            var fromCastle = _castles.FirstOrDefault(c => c.Id == e.Marker.Snippet);
            if (fromCastle == null)
                return;
            var option = new MarkerOptions();
            option.SetPosition(new LatLng(fromCastle.Position.Lat.Value, fromCastle.Position.Lng.Value));
            mHandler.RemoveCallbacks(mAnimation);
            _tempMarker = _map.AddMarker(option);
            SetupMarkerIcon(_tempMarker, GetIcon(fromCastle));
            mAnimation = new BounceAnimation(_tempMarker, mHandler);
            mHandler.Post(mAnimation);
        }

        private float _zoomLevel = 0;
        private void _map_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            if (_map.CameraPosition.Zoom == _zoomLevel)
                return;
            _zoomLevel = _map.CameraPosition.Zoom;
            foreach (var marker in _markerInstanceList)
            {
                var data = GetCustomPin(marker.Value);
                var icon = GetIcon(data);
                if (data == null || IsUriIcon(icon))
                    continue;
                Task.Factory.StartNew(() =>
                {
                    BitmapDescriptor iconDescriptor = null;
                    iconDescriptor = GetBitmapByZoomLevel(icon, _zoomLevel);
                    if (icon != null)
                        RunOnUiThread(() =>
                        {
                            marker.Value.SetIcon(iconDescriptor);
                        });
                });
            }
            //if (FormMap?.MyPosition != null)
            //    UpdateMyLocationMarker(FormMap.MyPosition);
        }
        CastleStateModel GetCustomPin(Marker annotation)
        {
            if (annotation == null)
                return null;
            return _castles.FirstOrDefault(e => e.Id == annotation.Snippet);
        }
        private void _map_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            if (_tempMarker != null)
            {
                mHandler.RemoveCallbacks(mAnimation);
                _tempMarker.Remove();
            }
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
                polyline.InvokeColor(Android.Graphics.Color.Red);
                _polylines.Add(_map.AddPolyline(polyline));
            }
            ViewModel.CastleClicked(e.Marker.Snippet);
            _bottom.ShowPane();
        }
    }
    [Register("virtualrisks.mobiles.droid.views.BounceAnimation")]
    public class BounceAnimation : Java.Lang.Object, IRunnable
    {
        private long mStart, mDuration;
        private IInterpolator mInterpolator;
        private Marker mMarker;
        private Handler mHandler;

        public BounceAnimation(Marker marker, Handler handler)
        {
            mMarker = marker;
            mHandler = handler;
            mInterpolator = new BounceInterpolator();
            mStart = SystemClock.UptimeMillis();
            mDuration = 1500L;
        }

        public void Run()
        {
            long elapsed = SystemClock.UptimeMillis() - mStart;
            float t = System.Math.Max(1 - mInterpolator.GetInterpolation((float)elapsed / mDuration), 0f);
            mMarker.SetAnchor(0.5f, 1f + 0.5f * t);
            if (t > 0.0)
            {
                // Post again 16ms later.
                mHandler.PostDelayed(this, 16L);
            }
            else
            {
                mStart = SystemClock.UptimeMillis();
                mHandler.Post(this);
            }
        }
    }
}

