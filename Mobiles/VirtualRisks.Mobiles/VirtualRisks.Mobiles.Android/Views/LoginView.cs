using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Library;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Attributes;
using VirtualRisks.Mobiles.Droid.utils;
using VirtualRisks.Mobiles.ViewModels;
using MvvmCross.Droid.Support.V4;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Object = Java.Lang.Object;
using Xamarin.Auth;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;

namespace VirtualRisks.Mobiles.Droid.Views
{
    [MvxDialogFragmentPresentation]
    [Activity(Label = "View for MainViewModel", Theme = "@style/Theme.Main")]
    public class LoginView : MvxDialogFragment<LoginViewModel>
    {
        private ActionProcessButton _btLogin;

        private IMvxInteraction<bool> _loginInteraction;

        public IMvxInteraction<bool> LoginInteraction
        {
            get { return _loginInteraction; }
            set
            {
                if (_loginInteraction != null)
                    _loginInteraction.Requested -= OnInteractionRequested;

                _loginInteraction = value;
                _loginInteraction.Requested += OnInteractionRequested;
            }
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<bool> eventArgs)
        {
            _btLogin.setProgress(eventArgs.Value ? 100 : -1);
        }

        public LoginView()
        {
        }

        protected LoginView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Cancelable = false;
            Dialog.Window.Attributes.WindowAnimations = Resource.Animation.modal_in;
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.LoginView, null);

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(this).For(v => v.LoginInteraction).To(viewModel => viewModel.LoginInteraction).OneWay();
            set.Apply();


            _btLogin = view.FindViewById<ActionProcessButton>(Resource.Id.btLogin);
            _btLogin.SetBackgroundColor(Color.ParseColor("#3b5999"));
            _btLogin.setMode(ActionProcessButton.Mode.ENDLESS);
            _btLogin.Click += (sender, args) =>
            {
                _btLogin.setProgress(1);
                var token = SilentLogin();
                if (string.IsNullOrEmpty(token))
                    FacebookLogin();
                else
                    TokenSuccess(token);
            };
            return view;
        }

        private string SilentLogin()
        {
            if (AccessToken.CurrentAccessToken == null || AccessToken.CurrentAccessToken.IsExpired)
                return null;
            var token = AccessToken.CurrentAccessToken == null
                ? null
                : AccessToken.CurrentAccessToken.Token;
            return token;
        }

        private void FacebookLogin()
        {
            var facebookCallback = new FacebookCallback<LoginResult>()
            {
                HandleSuccess = loginResult =>
                {
                    if (loginResult.AccessToken != null)
                    {
                        var props = new Dictionary<string, string>();
                        props.Add("access_token", loginResult.AccessToken.Token);
                        AccountStore.Create(Activity).Save(new Xamarin.Auth.Account(loginResult.AccessToken.UserId, props), "Facebook");
                        TokenSuccess(loginResult.AccessToken.Token);
                    }
                    else
                    {
                        TokenFailed();
                    }
                },

                HandleCancel = () =>
                {
                    TokenCancelled();
                },

                HandleError = exception =>
                {
                    TokenFailed();
                }
            };
            LoginManager.Instance.RegisterCallback(SocialLoginDroid.CallbackManager, facebookCallback);

            LoginManager.Instance.LogInWithReadPermissions(Activity,
                new List<string>()
                {
                    "email,user_friends,publish_actions,user_birthday,user_events,user_likes"
                });
        }

        private void TokenCancelled()
        {

        }

        private void TokenFailed()
        {
            _btLogin.setProgress(-1);
        }

        private void TokenSuccess(string token)
        {
            ViewModel.TokenSuccess(token);
        }
    }
    class FacebookCallback<TResult> : Object, IFacebookCallback where TResult : Object
    {
        public Action HandleCancel { get; set; }
        public Action<FacebookException> HandleError { get; set; }
        public Action<TResult> HandleSuccess { get; set; }

        public void OnCancel()
        {
            var c = HandleCancel;
            c?.Invoke();
        }

        public void OnError(FacebookException error)
        {
            var c = HandleError;
            c?.Invoke(error);
        }

        public void OnSuccess(Object result)
        {
            var c = HandleSuccess;
            c?.Invoke(JavaObjectExtensions.JavaCast<TResult>(result));
        }
    }
}