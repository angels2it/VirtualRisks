using Autofac;
using Autofac.Integration.WebApi;
using CastleGo.Domain;
using CastleGo.WebApi.Providers;
using MongoRepository;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using CastleGo.Common.Modules;
using CastleGo.WebApi.Directions;
using CastleGo.WebApi.Workers;
using Hangfire;
using EventHandler = CastleGo.Domain.EventHandler;
using FirebaseSharp.Portable;
using GoogleMapsAPI.NET.API.Client;

namespace CastleGo.WebApi
{
    /// <summary>Represent Autofac configuration.</summary>
    public static class AutofacConfig
    {
        /// <summary>
        /// Configured instance of <see cref="T:Autofac.IContainer" />
        /// <remarks><see>
        ///         <cref>AutofacConfig.Configure</cref>
        ///     </see>
        ///     must be called before trying to get Container instance.</remarks>
        /// </summary>
        public static IContainer Container;

        /// <summary>
        /// Initializes and configures instance of <see cref="T:Autofac.IContainer" />.
        /// </summary>
        /// <param name="configuration"></param>
        public static HttpConfiguration UsingAutofac(this HttpConfiguration configuration)
        {
            ContainerBuilder builder = new ContainerBuilder();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith("Provider")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith("HangfireJob")).AsSelf().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith("Factory")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(e => e.IsAssignableTo<IWorker>())
                .As<IWorker>()
                .Named<IWorker>(type => type.Name)
                .InstancePerLifetimeScope();

            builder.RegisterInstance(new FirebaseApp(new Uri(ConfigurationManager.AppSettings["FirebaseUrl"]))).SingleInstance();

            builder.RegisterInstance(new MapsAPIClient(ConfigurationManager.AppSettings["GoogleApiKey"])).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CastleDirectionService>().AsImplementedInterfaces();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<AdminOAuthAuthorizationProvider>().AsSelf().SingleInstance();

            builder.UsingEventStore();

            builder.RegisterModule(new SettingsModule(ConfigurationManager.AppSettings));

            //builder.RegisterInstance(ConfigurationManager.GetSection("troopTypes") as TroopTypesCollection);

            Container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            EventHandler.Init(Container);
            MessageHandler.Init(Container);
            Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(Container, false);
            return configuration;
        }
    }
}
