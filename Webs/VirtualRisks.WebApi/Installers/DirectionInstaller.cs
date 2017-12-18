using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GoogleMapsAPI.NET.API.Client;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using VirtualRisks.WebApi.Directions;

namespace VirtualRisks.WebApi.Installers
{
    public class DirectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMapsAPIClient>().Instance(new MapsAPIClient("AIzaSyCdXQtmFxmrLCy0bhXw4IcwWmlcflN63_A")));
            container.Register(Component.For<ICastleDirectionService>().ImplementedBy<CastleDirectionService>());
        }
    }

}