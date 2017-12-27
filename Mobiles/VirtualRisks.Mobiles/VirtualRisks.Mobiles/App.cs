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
            var api = new VirtualRisksAPI()
            {
                BaseUri = new Uri("http://192.168.0.10:64545/"),
            };
            api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER", Settings.Token);
            Mvx.RegisterType<IVirtualRisksAPI>(() => api);
        }
    }
}
