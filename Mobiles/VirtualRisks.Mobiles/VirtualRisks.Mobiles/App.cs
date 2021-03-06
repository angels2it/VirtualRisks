using System;
using System.Net.Http.Headers;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using VirtualRisks.WebApi.RestClient;
using Acr.UserDialogs;
using Akavache;
using VirtualRisks.Mobiles.Helpers;
using VirtualRisks.Mobiles.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace VirtualRisks.Mobiles
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            BlobCache.ApplicationName = "VirtualRisks";
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.LazyConstructAndRegisterSingleton<IMvxMessenger, MvxMessengerHub>();

            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            Mvx.RegisterSingleton<ICommonViewResource>(new CommonViewResource());
            Mvx.RegisterSingleton<IGamePageViewResource>(new GamePageViewResource());
            InitRestClient();
            if (Settings.RequireLogin())
                Settings.Clear();
            RegisterAppStart<ViewModels.MainViewModel>();
        }

        public static void InitRestClient()
        {
#if !DEBUG
            var url = "http://118.139.163.66:9910/";
#else
            var url = "http://192.168.0.10:64545/";
            //var url = "http://118.139.163.66:9910/";
#endif
            var api = new VirtualRisksAPI()
            {
                BaseUri = new Uri(url),
            };
            api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER", Settings.Token);
            Mvx.RegisterType<IVirtualRisksAPI>(() => api);
        }
    }
}
