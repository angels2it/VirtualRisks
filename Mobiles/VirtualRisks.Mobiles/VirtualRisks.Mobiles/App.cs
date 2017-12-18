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

            Mvx.RegisterType<IVirtualRisksWebApi>(() => new VirtualRisksWebApi()
            {
                BaseUri = new Uri("http://192.168.0.116:8088/")
            });
            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
