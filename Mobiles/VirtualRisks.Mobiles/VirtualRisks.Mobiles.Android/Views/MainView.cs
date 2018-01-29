using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using VirtualRisks.Mobiles.ViewModels;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using MvvmCross.Binding.BindingContext;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Java.Net;
using Java.IO;
using Java.Lang;
using Cheesebaron.SlidingUpPanel;
using VirtualRisks.Mobiles.Helpers;
using Android.Animation;
using System;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using CastleGo.Shared.Common;
using Com.Airbnb.Lottie;
using VirtualRisks.Mobiles.Droid.Animations;
using CastleGo.Shared.Games.Events;
using CastleGo.Shared.Games;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class MainView : MvxFragmentActivity<MainViewModel>, IOnMapReadyCallback
    {
        private const int MOVE_SOLDIER_DISTANCE = 50;
        private MvxFluentBindingDescriptionSet<MainView, MainViewModel> _set;
        private readonly Dictionary<MarkerInfo, Marker> _markerInstanceList = new Dictionary<MarkerInfo, Marker>();
        private BitmapDescriptor _dotIcon;

        private Handler mHandler;
        private IRunnable mAnimation;

        private GoogleMap _map;
        private ProgressBar _pbLoading;

        private View _fabEventMove;
        private TextView _fabEventText;
        private FloatingActionButton _fabEventButton;

        private TextView _fabText;
        private MovableFloatingActionButton _fabButton;
        private List<Polyline> _polylines = new List<Polyline>();
        private List<Marker> _dots = new List<Marker>();
        private IMvxInteraction<GameStateUpdate> _gameInit;
        public IMvxInteraction<GameStateUpdate> GameInit
        {
            get => _gameInit;
            set
            {
                if (_gameInit != null)
                    _gameInit.Requested -= OnGameInitRequested;

                _gameInit = value;
                _gameInit.Requested += OnGameInitRequested;
            }
        }

        private IMvxInteraction<GameStateUpdate> _gameUpdate;
        public IMvxInteraction<GameStateUpdate> GameUpdate
        {
            get => _gameUpdate;
            set
            {
                if (_gameUpdate != null)
                    _gameUpdate.Requested -= OnGameUpdateRequested;

                _gameUpdate = value;
                _gameUpdate.Requested += OnGameUpdateRequested;
            }
        }


        private IMvxInteraction<BattalionMovementEventModel> _battalionAdded;
        public IMvxInteraction<BattalionMovementEventModel> BattalionAdded
        {
            get => _battalionAdded;
            set
            {
                if (_battalionAdded != null)
                    _battalionAdded.Requested -= OnBattalionAddedRequested;

                _battalionAdded = value;
                _battalionAdded.Requested += OnBattalionAddedRequested;
            }
        }

        private void OnBattalionAddedRequested(object sender, MvxValueEventArgs<BattalionMovementEventModel> e)
        {
            var fromCastle = ViewModel.State.Castles.FirstOrDefault(c => c.Id == e.Value.CastleId.ToString());
            if (fromCastle == null)
                return;
            var option = new MarkerOptions();
            option.SetTitle("Tank");
            option.SetSnippet(e.Value.Id.ToString());
            option.Draggable(false);
            option.InvokeZIndex(101);
            var latlng = new LatLng(fromCastle.Position.Lat, fromCastle.Position.Lng);
            option.SetPosition(latlng);
            option.Anchor(0.5f, 0.5f);
            var marker = _map.AddMarker(option);
            if (!_markerInstanceList.Any(m => m.Key.Key == marker.Snippet))
                _markerInstanceList.Add(new MarkerInfo(MarkerType.Tank, marker.Snippet), marker);
            SetupMarkerIcon(marker, "marker_tank_blue");
            new MarkerMovingAnimation(_map, marker)
                .AnimateMarker(e.Value.Route.Duration.TotalMilliseconds,
                    latlng,
                    e.Value.Positions.Where(p => p.Lat != latlng.Latitude && p.Lng != latlng.Longitude).Select(f => new LatLng(f.Lat, f.Lng)).ToList(), 0);
        }
        private void OnGameUpdateRequested(object sender, MvxValueEventArgs<GameStateUpdate> eventArgs)
        {
            RunOnUiThread(() =>
            {
                _fabText.Text = ViewModel.State.GetSoldiersAmount().ToString();
                foreach (var castle in eventArgs.Value.Castles)
                {
                    if (!_markerInstanceList.Any(m => m.Key.Key == castle.Id))
                        continue;
                    var marker = _markerInstanceList.First(m => m.Key.Key == castle.Id);
                    SetupMarkerIcon(marker.Value, GetIcon(castle));
                }
            });
        }
        private void OnGameInitRequested(object sender, MvxValueEventArgs<GameStateUpdate> eventArgs)
        {
            RunOnUiThread(() =>
            {
                _fabText.Text = ViewModel.State.GetSoldiersAmount().ToString();
                _map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(eventArgs.Value.Castles[0].Position.Lat,
                    eventArgs.Value.Castles[0].Position.Lng), 15));
                foreach (var castle in eventArgs.Value.Castles)
                {
                    var option = new MarkerOptions();
                    option.Anchor(0.5f, 0.5f);
                    option.SetTitle("Castle " + castle.Name);
                    option.SetSnippet(castle.Id);
                    option.Draggable(castle.Army == Army.Blue);
                    option.InvokeZIndex(2);
                    var latlng = new LatLng(castle.Position.Lat, castle.Position.Lng);
                    var startPoint = _map.Projection.ToScreenLocation(latlng);
                    startPoint.Y = 0;
                    var startLocation = _map.Projection.FromScreenLocation(startPoint);
                    option.SetPosition(latlng);
                    var marker = _map.AddMarker(option);
                    if (!_markerInstanceList.Any(m => m.Key.Key == marker.Snippet))
                        _markerInstanceList.Add(new MarkerInfo(MarkerType.Castle, marker.Snippet, castle), marker);
                    SetupMarkerIcon(marker, GetIcon(castle));
                    new BounceInAnimation(marker, startLocation, latlng).Run();
                }
            });
        }

        private string GetIcon(CastleStateModel castle)
        {
            if (castle == null)
                return "ic_splash";
            return castle.Army == Army.Red ? "red_castle" : "blue_castle";
        }
        private void SetDefaultIcon(Marker marker)
        {
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
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
            if (Utils.IsUriIcon(icon))
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

        private LottieAnimationView _loadingView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            _dotIcon = BitmapDescriptorFactory.FromResource(Resource.Drawable.dot);
            var mainContent = _view.FindViewById<RelativeLayout>(Resource.Id.main_content);
            _fabText = _view.FindViewById<TextView>(Resource.Id.fabText);
            _fabButton = _view.FindViewById<MovableFloatingActionButton>(Resource.Id.fabBtn);
            _fabButton.SetMovableView(_fabButton.Parent.Parent as View);
            _fabButton.LongClick += _fabButton_LongClick            ;

            _fabButton.Click += _fabButton_Click;
            _fabButton.Dragging += _fabButton_Dragging;
            _fabButton.DragStart += _fabButton_DragStart;
            _fabButton.DragEnd += _fabButton_DragEnd;

            _fabEventText = _view.FindViewById<TextView>(Resource.Id.fabEventText);
            _fabEventButton = _view.FindViewById<FloatingActionButton>(Resource.Id.fabEventBtn);
            _fabEventMove = (View) _fabEventButton.Parent;
            _fabEventMove.Animate()
                .TranslationY(150)
                .ScaleX(0)
                .ScaleY(0)
                .SetDuration(0)
                .Start();
            _fabEventButton.Click += _fabEventButton_Click;

            _loadingView = new LottieAnimationView(this);
            _loadingView.SetBackgroundColor(Color.White);
            _loadingView.Loop(true);
            _loadingView.SetAnimation("splash.json");
            _loadingView.LayoutParameters = new RelativeLayout.LayoutParams(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.MatchParent);
            mainContent.AddView(_loadingView);
            _loadingView.PlayAnimation();
            mHandler = new Handler();
            SocialLoginDroid.Init(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
            _set = this.CreateBindingSet<MainView, MainViewModel>();
            _set.Bind(this).For(view => view.GameInit).To(viewModel => viewModel.GameInit).OneWay();
            _set.Bind(this).For(view => view.GameUpdate).To(viewModel => viewModel.GameUpdate).OneWay();
            _set.Bind(this).For(view => view.BattalionAdded).To(viewModel => viewModel.BattalionAdded).OneWay();
            SetLoadingControl();
            SetBottomSheet();
            _set.Apply();
            var map = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            map.GetMapAsync(this);
        }

        private void _fabButton_LongClick(object sender, View.LongClickEventArgs e)
        {
            _fabEventMove.Animate()
                .TranslationY(0)
                .ScaleX(1)
                .ScaleY(1)
                .SetDuration(1500)
                .Start();
        }

        private void _fabEventButton_Click(object sender, EventArgs e)
        {
            ViewModel.ShowEvents();
        }

        private void _fabButton_DragStart(object sender, FabDragEvent e)
        {
            var dragAbleCastles = ViewModel.State.GetOpponentCastlesId();
            var fadedOutMarkers = _markerInstanceList.Where(m => dragAbleCastles.Contains(m.Key.Key));
            foreach (var marker in fadedOutMarkers)
            {
                FadeOutMarker(marker.Value);
            }
        }

        private void _fabButton_Dragging(object sender, FabDragEvent e)
        {
            var toPoint = new Point((int)e.X, (int)e.Y);

            var latlng = _map.Projection.FromScreenLocation(toPoint);
            var dragAbleCastles = ViewModel.State.GetMyCastlesId();
            var dragableMarker = _markerInstanceList.Where(m => dragAbleCastles.Contains(m.Key.Key));
            foreach (var marker in dragableMarker)
            {
                SetupMarkerIcon(marker.Value, GetIcon(marker.Key.Object as CastleStateModel));
            }

            var nearestCastle = ViewModel.State.GetNearestCastle(dragAbleCastles, latlng.Latitude, latlng.Longitude);
            if (nearestCastle.Distance * 1000 > MOVE_SOLDIER_DISTANCE)
                return;
            var nearestMarker = _markerInstanceList.FirstOrDefault(m => m.Key.Key == nearestCastle.Castle.Id);
            SetupMarkerIcon(nearestMarker.Value, "ic_war");
        }

        private void _fabButton_DragEnd(object sender, FabDragEvent e)
        {
            var opponentCastlesId = ViewModel.State.GetOpponentCastlesId();
            var fadeInMarker = _markerInstanceList.Where(m => opponentCastlesId.Contains(m.Key.Key));
            foreach (var marker in fadeInMarker)
            {
                FadeInMarker(marker.Value);
            }
            var myCastles = ViewModel.State.GetMyCastlesId();
            var dragableMarker = _markerInstanceList.Where(m => myCastles.Contains(m.Key.Key));
            foreach (var marker in dragableMarker)
            {
                SetupMarkerIcon(marker.Value, GetIcon(marker.Key.Object as CastleStateModel));
            }
            var toPoint = new Point((int)e.X, (int)e.Y);
            var latlng = _map.Projection.FromScreenLocation(toPoint);
            var nearestCastle = ViewModel.State.GetNearestCastle(myCastles, latlng.Latitude, latlng.Longitude);
            if (nearestCastle.Distance * 1000 > MOVE_SOLDIER_DISTANCE)
                return;
            var nearestMarker = _markerInstanceList.FirstOrDefault(m => m.Key.Key == nearestCastle.Castle.Id);
            ViewModel.MoveSoldiers(nearestMarker.Key.Key);
            _fabText.Text = "0";
        }

        private void _fabButton_Click(object sender, EventArgs e)
        {
            ViewModel.ShowMySoldiers();
        }

        private SlidingUpPanelLayout _bottom;
        public void SetBottomSheet()
        {
            _bottom = FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
            _bottom.ShadowDrawable = Resources.GetDrawable(Resource.Drawable.above_shadow);
            _bottom.AnchorPoint = 0.3f;
            _bottom.PanelHeight = 100;
            _bottom.PanelCollapsed += _bottom_PanelCollapsed;
            _bottom.PanelAnchored += _bottom_PanelAnchored;
            var slider = _bottom.GetChildAt(1);
            slider.Visibility = ViewStates.Gone;


            var viewDetail = SupportFragmentManager.FindFragmentById(Resource.Id.fragDetail) as CastleView;
        }

        private void _bottom_PanelAnchored(object sender, SlidingUpPanelEventArgs args)
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
            _map.MarkerDrag += _map_MarkerDrag;
            _map.CameraChange += _map_CameraChange;
            ViewModel.InitGame().ContinueWith(r =>
            {
                RunOnUiThread(() =>
                {
                    _loadingView.Visibility = ViewStates.Gone;
                });
            });
        }

        private void _map_MarkerDrag(object sender, GoogleMap.MarkerDragEventArgs e)
        {
            var dragAbleCastles = ViewModel.State.GetDragableCastles(e.Marker.Snippet);
            var dragableMarker = _markerInstanceList.Where(m => dragAbleCastles.Contains(m.Key.Key));
            foreach (var marker in dragableMarker)
            {
                SetupMarkerIcon(marker.Value, GetIcon(marker.Key.Object as CastleStateModel));
            }

            var nearestCastle = ViewModel.State.GetNearestCastle(dragAbleCastles, e.Marker.Position.Latitude, e.Marker.Position.Longitude);
            if (nearestCastle.Distance * 1000 > 50)
                return;
            var nearestMarker = _markerInstanceList.FirstOrDefault(m => m.Key.Key == nearestCastle.Castle.Id);
            SetupMarkerIcon(nearestMarker.Value, "ic_war");
        }

        private Marker _tempMarker;
        private void _map_MarkerDragStart(object sender, GoogleMap.MarkerDragStartEventArgs e)
        {
            var fromCastle = ViewModel.State.Castles.FirstOrDefault(c => c.Id == e.Marker.Snippet);
            if (fromCastle == null)
                return;
            e.Marker.ZIndex = 99;
            SetupMarkerIcon(e.Marker, "ic_movesoldier");
            var dragAbleCastles = ViewModel.State.GetDragableCastles(e.Marker.Snippet);
            var fadedOutMarkers = _markerInstanceList.Where(m => !dragAbleCastles.Contains(m.Key.Key) && m.Key.Key != e.Marker.Snippet);
            foreach (var marker in fadedOutMarkers)
            {
                FadeOutMarker(marker.Value);
            }

            var option = new MarkerOptions();
            option.SetPosition(new LatLng(fromCastle.Position.Lat, fromCastle.Position.Lng));
            mHandler.RemoveCallbacks(mAnimation);
            _tempMarker = _map.AddMarker(option);
            SetupMarkerIcon(_tempMarker, "ic_soldier");
            mAnimation = new BounceAnimation(_tempMarker, mHandler);
            mHandler.Post(mAnimation);
        }

        private void FadeInMarker(Marker marker)
        {
            var animator = ObjectAnimator.OfFloat(marker, "Alpha", 0f, 1f);
            animator.SetDuration(500);
            animator.Start();
        }
        private void FadeOutMarker(Marker marker)
        {
            var animator = ObjectAnimator.OfFloat(marker, "Alpha", 1f, 0f);
            animator.SetDuration(500);
            animator.Start();
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
                if (data == null)
                    continue;
                var icon = GetIcon(data);
                if (Utils.IsUriIcon(icon))
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
        }
        CastleStateModel GetCustomPin(Marker annotation)
        {
            if (annotation == null)
                return null;
            return ViewModel.State.Castles.FirstOrDefault(e => e.Id == annotation.Snippet);
        }
        private void _map_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            if (_tempMarker != null)
            {
                mHandler.RemoveCallbacks(mAnimation);
                _tempMarker.Remove();
            }
            var fromCastle = ViewModel.State.Castles.FirstOrDefault(c => c.Id == e.Marker.Snippet);
            if (fromCastle == null)
                return;
            SetupMarkerIcon(e.Marker, GetIcon(fromCastle));
            var dragAbleCastles = ViewModel.State.GetDragableCastles(e.Marker.Snippet);
            var dragableMarker = _markerInstanceList.Where(m => dragAbleCastles.Contains(m.Key.Key));
            foreach (var marker in dragableMarker)
            {
                SetupMarkerIcon(marker.Value, GetIcon(marker.Key.Object as CastleStateModel));
            }
            var fadeInMarkers = _markerInstanceList.Where(m => !dragAbleCastles.Contains(m.Key.Key) && m.Key.Key != e.Marker.Snippet);
            foreach (var marker in fadeInMarkers)
            {
                FadeInMarker(marker.Value);
            }
            var nearestCastle = ViewModel.State.GetNearestCastle(dragAbleCastles, e.Marker.Position.Latitude, e.Marker.Position.Longitude);

            if (nearestCastle.Distance * 1000 > 50)
                ViewModel.DragCastleLargeDistance();
            else
                ViewModel.DragToCastle(e.Marker.Snippet, nearestCastle.Castle.Id);
            e.Marker.Position = new LatLng(fromCastle.Position.Lat, fromCastle.Position.Lng);
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
            var castle = ViewModel.State.Castles.FirstOrDefault(c => c.Id == e.Marker.Snippet);
            if (castle == null)
                return;
            foreach (var polyline in _polylines)
            {
                polyline.Remove();
            }
            foreach (var dot in _dots)
            {
                dot.Remove();
            }
            _dots.Clear();

            var routes = ViewModel.State.Routes.Where(r => r.FromCastle == e.Marker.Snippet || r.ToCastle == e.Marker.Snippet);
            if (routes == null)
                return;

            foreach (var route in routes)
            {
                if (route?.FormattedRoute.Count == 0)
                    continue;
                var polyline = new PolylineOptions();
                polyline.Add(e.Marker.Position);
                foreach (var step in route.FormattedRoute)
                {
                    polyline.Add(new LatLng(step.Lat, step.Lng));
                }
                polyline.InvokeColor(Android.Graphics.Color.Red);
                _polylines.Add(_map.AddPolyline(polyline));
                var markerOptions = new MarkerOptions();
                markerOptions.SetIcon(_dotIcon);
                markerOptions.Anchor(0.5f, 0.5f);
                markerOptions.InvokeZIndex(1);
                var location = route.FormattedRoute.First();
                markerOptions.SetPosition(new LatLng(location.Lat, location.Lng));
                _dots.Add(_map.AddMarker(markerOptions));
                location = route.FormattedRoute.Last();
                markerOptions.SetPosition(new LatLng(location.Lat, location.Lng));
                _dots.Add(_map.AddMarker(markerOptions));
            }
            ViewModel.CastleClicked(e.Marker.Snippet);
            _bottom.ShowPane();
        }
    }
}

