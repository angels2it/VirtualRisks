using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using VirtualRisks.Mobiles.ViewModels;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class NewGameView : MvxDialogFragment<NewGameViewModel>
    {
        public NewGameView()
        {
        }

        protected NewGameView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Cancelable = false;
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.NewGame, null);
            var grid = (MvxGridView)view.FindViewById(Resource.Id.grid);
            grid.Adapter = new GridPlayerAdapter(Context, (IMvxAndroidBindingContext)BindingContext);
            return view;
        }


        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);
        //    SetContentView(Resource.Layout.NewGame);
        //    var grid = (MvxGridView)FindViewById(Resource.Id.grid);
        //    grid.Adapter = new GridPlayerAdapter(this, (IMvxAndroidBindingContext)BindingContext);
        //}
    }
    public class GridPlayerAdapter : MvxAdapter<PlayerModel>
    {
        public GridPlayerAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            var context = (PlayerModel)dataContext;
            if (context.IsAddButton)
                templateId = Resource.Layout.item_add_player;
            else
                templateId = Resource.Layout.item_player;
            return base.GetBindableView(convertView, dataContext, parent, templateId);
        }
    }
}

