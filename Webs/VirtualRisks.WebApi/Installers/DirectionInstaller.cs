using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GoogleMapsAPI.NET.API.Client;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using System.Configuration;
using VirtualRisks.WebApi.Directions;

namespace VirtualRisks.WebApi.Installers
{
    public class DirectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMapsAPIClient>().Instance(new MapsAPIClient(ConfigurationManager.AppSettings["GoogleApiKey"])));
            container.Register(Component.For<ICastleDirectionService>().ImplementedBy<CastleDirectionService>());
        }
    }

}