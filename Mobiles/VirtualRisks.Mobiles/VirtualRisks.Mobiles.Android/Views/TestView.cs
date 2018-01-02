using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Cheesebaron.SlidingUpPanel;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.Design;
using MvvmCross.Droid.Views.Attributes;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class TestView : MvxBottomSheetDialogFragment
    {
        public TestView()
        {
        }

        protected TestView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.TestView, null);
            var layout = view.FindViewById<SlidingUpPanelLayout>(Resource.Id.sliding_layout);
            layout.ShadowDrawable = Resources.GetDrawable(Resource.Drawable.above_shadow);
            layout.AnchorPoint = 0.3f;
            layout.PanelExpanded += (s, e) => Log.Info(Tag, "PanelExpanded");
            layout.PanelCollapsed += (s, e) => Log.Info(Tag, "PanelCollapsed");
            layout.PanelAnchored += (s, e) => Log.Info(Tag, "PanelAnchored");
            layout.PanelSlide += (s, e) =>
            {
                
            };
            return view;
        }
    }
}