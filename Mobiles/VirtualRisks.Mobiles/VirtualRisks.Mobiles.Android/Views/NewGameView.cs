using System;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Library;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using VirtualRisks.Mobiles.Droid.utils;
using VirtualRisks.Mobiles.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Core;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class NewGameView : MvxDialogFragment<NewGameViewModel>, ProgressGenerator.OnCompleteListener
    {
        private ActionProcessButton _btnUpload;
        private IMvxInteraction<bool> _createGameInteraction;

        public IMvxInteraction<bool> CreateGameInteraction
        {
            get { return _createGameInteraction; }
            set
            {
                if (_createGameInteraction != null)
                    _createGameInteraction.Requested -= OnInteractionRequested;

                _createGameInteraction = value;
                _createGameInteraction.Requested += OnInteractionRequested;
            }
        }
        private void OnInteractionRequested(object sender, MvxValueEventArgs<bool> eventArgs)
        {
            Activity.RunOnUiThread(() =>
            {
                if (eventArgs.Value)
                {
                    _btnUpload.setProgress(100);
                    _btnUpload.Enabled = false;
                }
                else
                {
                    _btnUpload.setProgress(-1);
                }
            });
        }
        public NewGameView()
        {
        }

        protected NewGameView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Cancelable = false;
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var set = this.CreateBindingSet<NewGameView, NewGameViewModel>();
            set.Bind(this).For(v => v.CreateGameInteraction).To(viewModel => viewModel.CreateGameInteraction).OneWay();
            set.Apply();


            var view = this.BindingInflate(Resource.Layout.NewGame, null);
            // grid
            var grid = (MvxGridView)view.FindViewById(Resource.Id.grid);
            grid.Adapter = new GridPlayerAdapter(Context, (IMvxAndroidBindingContext)BindingContext);

            // button
            ProgressGenerator progressGenerator = new ProgressGenerator(this);
            _btnUpload = view.FindViewById<ActionProcessButton>(Resource.Id.btNext);
            _btnUpload.setMode(ActionProcessButton.Mode.ENDLESS);
            _btnUpload.Click += (object sender, EventArgs e) =>
            {
                //progressGenerator.start(btnUpload, Activity);
                _btnUpload.setProgress(1);
                ViewModel.NextCommand.Execute(null);
            };

            return view;
        }
        #region OnCompleteListener implementation

        void ProgressGenerator.OnCompleteListener.onComplete()
        {

        }

        #endregion
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

