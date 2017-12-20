using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Views;
using VirtualRisks.Mobiles.ViewModels;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [Activity(Label = "View for NewGameViewModel")]
    public class NewGameView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NewGame);
            var grid = (MvxGridView)FindViewById(Resource.Id.grid);
            grid.Adapter = new GridPlayerAdapter(this, (IMvxAndroidBindingContext)BindingContext);
        }
    }
    public class GridPlayerAdapter : MvxAdapter<PlayerModel>
    {
        public GridPlayerAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            var context = (PlayerModel) dataContext;
            if (context.IsAddButton)
                templateId = Resource.Layout.item_add_player;
            else
                templateId = Resource.Layout.item_player;
            return base.GetBindableView(convertView, dataContext, parent,templateId);
        }
    }
}

