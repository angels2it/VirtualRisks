using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CastleGo.Shared.Common;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Views.Fragments;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using VirtualRisks.Mobiles.ViewModels;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Layout.CastleView, fragmentHostViewType: typeof(CastleView))]
    public class CastleView : MvvmCross.Droid.Support.V4.MvxFragment
    {
        private MvxFluentBindingDescriptionSet<CastleView, MainViewModel> _set;
        private ProgressBar _pbLoading;
        public CastleView()
        {
        }

        protected CastleView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
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
            Activity.RunOnUiThread(() =>
            {
                _pbLoading.Visibility = eventArgs.Value ? ViewStates.Visible : ViewStates.Gone;
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.CastleView, null);
            _pbLoading = view.FindViewById<ProgressBar>(Resource.Id.pbLoading);
            _pbLoading.Indeterminate = true;
            _pbLoading.IndeterminateTintList = ColorStateList.ValueOf(Android.Graphics.Color.Red);
            ViewModel = HostViewModel;
            if (HostViewModel == null)
                return view;
            _set = this.CreateBindingSet<CastleView, MainViewModel>();
            _set.Bind(this).For(v => v.Loading).To(viewModel => viewModel.Loading).OneWay();
            _set.Apply();
            // grid
            var grid = (MvxListView)view.FindViewById(Resource.Id.listSoldier);
            //grid.ItemsSource = HostViewModel.Items;
            grid.Adapter = new ListSoldierAdapter(Context, (IMvxAndroidBindingContext)HostView.BindingContext);
            return view;
        }
        MainView HostView => Host as MainView;
        private MainViewModel HostViewModel => (Host as MainView)?.ViewModel;
        internal void Init(string id)
        {
            HostViewModel?.InitCastleDetail(id);
        }
    }

    public class ListSoldierAdapter : MvxAdapter<SoldierModel>
    {
        public ListSoldierAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            var context = (SoldierItemModel)dataContext;
            templateId = Resource.Layout.item_soldier;
            var view= base.GetBindableView(convertView, dataContext, parent, templateId);
            var img = view.FindViewById<ImageView>(Resource.Id.imgSoldier);
            if (context.Army == Army.Red)
            {
                img.SetImageResource(Resource.Drawable.soldier_red);
            }
            else
            {
                img.SetImageResource(Resource.Drawable.soldier_blue);
            }

            return view;
        }
    }
}