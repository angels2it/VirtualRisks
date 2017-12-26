using System;
using System.Net.Http.Headers;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using VirtualRisks.WebApi.RestClient;
using Acr.UserDialogs;

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
            var api = new VirtualRisksAPI()
            {
                BaseUri = new Uri("http://192.168.0.10:64545/"),
            };
            api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BEARER", "aKppwJo8AWsBD6tF5B-l-1Sq9AVxJObneZuEpQPBYcU0aGmugOCPxCVs9UQXDviYpM1n3LboJwF16VVk7j84pM0XEq5Ly7pKngA_vGWmdLXXXtxvE1-QL3Oz7QGTC2aA-KQiZgm-FbnVO9PJ7BXiY4S7qyvsr5pRwX3iOTTEw3xb9bYhEgMLRJa8fbkP4tkAZJe0EK_MfX7qLvUvCRPH0PfPfflY9TpbZ0px4TXkkcxOuFnTlQZYv7F8MpGLS0SSFbLGvpJMI3l19c0b0LSabwfI-ahvmsycO5iDg0wgccPB9qzeHwDmOCDhXA7EtGqqxuELFTx3diePBNk01HcIgdc1At_iZoamrHs7TcgiwGY");
            Mvx.RegisterType<IVirtualRisksAPI>(() => api);
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
