using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.Design;
using MvvmCross.Droid.Views.Attributes;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    public class CastleView : MvxBottomSheetDialogFragment
    {
        public CastleView()
        {
        }

        protected CastleView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Cancelable = false;
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.LoginView, null);
            return view;
        }
    }
}