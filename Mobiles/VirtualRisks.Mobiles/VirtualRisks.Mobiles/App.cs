using System;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using VirtualRisks.WebApi.RestClient;

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

            Mvx.RegisterType<IVirtualRisksAPI>(() => new VirtualRisksAPI()
            {
                BaseUri = new Uri("http://192.168.1.167:8088/")
            });
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
