using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.Design;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using VirtualRisks.Mobiles.ViewModels;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Activity(Label = "View for SoldiersViewModel", Theme = "@style/Theme.Main")]
    public class SoldiersView : MvxBottomSheetDialogFragment<SoldiersViewModel>
    {
        private MvxFluentBindingDescriptionSet<SoldiersView, SoldiersViewModel> _set;
        private ProgressBar _pbLoading;
        protected SoldiersView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        public SoldiersView()
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
            _pbLoading.Visibility = ViewStates.Gone;
            _set = this.CreateBindingSet<SoldiersView, SoldiersViewModel>();
            _set.Bind(this).For(v => v.Loading).To(viewModel => viewModel.Loading).OneWay();
            _set.Apply();
            // grid
            var grid = (MvxListView)view.FindViewById(Resource.Id.listSoldier);
            //grid.ItemsSource = HostViewModel.Items;
            grid.Adapter = new ListSoldierAdapter(Context, (IMvxAndroidBindingContext)BindingContext);
            return view;
        }
    }
}