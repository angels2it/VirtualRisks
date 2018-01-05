using System;
using System.Net.Http.Headers;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using VirtualRisks.WebApi.RestClient;
using Acr.UserDialogs;
using VirtualRisks.Mobiles.Helpers;

namespace VirtualRisks.Mobiles
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            InitRestClient();
            RegisterAppStart<ViewModels.MainViewModel>();
        }

        public static void InitRestClient()
        {
            //var url = "http://192.168.0.10:64545/";
            var url = "http://118.139.163.66:9910/";
            var api = new VirtualRisksAPI()
            {
                BaseUri = new Uri(url),
            };
            api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER", Settings.Token);
            Mvx.RegisterType<IVirtualRisksAPI>(() => api);
        }
    }
}
