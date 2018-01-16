using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json.Linq;
using VirtualRisks.Mobiles.Helpers;
using VirtualRisks.Mobiles.Models;
using VirtualRisks.WebApi.RestClient;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class LoginViewModel : MvxViewModel<LoginRequest, LoginResponse>
    {
        private readonly IVirtualRisksAPI _api;
        private readonly IMvxNavigationService _navigationService;
        public MvxInteraction<bool> LoginInteraction { get; } = new MvxInteraction<bool>();

        public LoginViewModel(IVirtualRisksAPI api, IMvxNavigationService navigationService)
        {
            _api = api;
            _navigationService = navigationService;
        }

        public void TokenSuccess(string token)
        {
            _api.Account.ObtainLocalAccessTokenAsync("Facebook", token)
                .ContinueWith(r =>
                {
                    var result = r.Result as JObject;
                    if(result == null)
                        return;
                    JToken accessToken;
                    if (result.TryGetValue("access_token", out accessToken))
                    {
                        Settings.Token = accessToken.Value<string>();
                        Settings.UserId = result.Value<string>("user_id");
                        App.InitRestClient();
                        _navigationService.Close(this, new LoginResponse());
                    }
                });
        }

        public override void Prepare(LoginRequest parameter)
        {
            
        }
    }
}
