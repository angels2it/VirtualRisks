// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Startup
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using Autofac;
using CastleGo.Domain.Bases;
using MongoDB.Bson.Serialization;
using NEventStore;
using NEventStore.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyMessenger;

namespace CastleGo.Domain
{
    public static class DomainStatup
    {
        private static readonly Dictionary<string, string> Libs = new Dictionary<string, string>() { { "MongoDB.Driver, Version=1.10.0.62, Culture=neutral, PublicKeyToken=f686731cfb9cc103", "Libs/MongoDB.Driver.dll" }, { "MongoDB.Bson, Version=1.10.0.62, Culture=neutral, PublicKeyToken=f686731cfb9cc103", "Libs/MongoDB.Bson.dll" } };

        public static void Init(this ContainerBuilder builder)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            BsonInit();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(e => e.Name.EndsWith("EventHandler")).AsImplementedInterfaces().InstancePerLifetimeScope();
            IStoreEvents store = InitEventStore();
            builder.RegisterInstance(store).As<IStoreEvents>();
            builder.RegisterType<DomainService>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(e => e.Name.EndsWith("MessageHandler")).AsImplementedInterfaces().InstancePerLifetimeScope();
            TinyMessengerHub instance2 = new TinyMessengerHub();
            builder.RegisterInstance(instance2).AsImplementedInterfaces().SingleInstance();
        }

        private static IStoreEvents InitEventStore()
        {
            PersistenceWireup wireup = Wireup.Init()
                .LogToOutputWindow()
                .UsingMongoPersistence("MongoServerSettings", new DocumentObjectSerializer())
                .InitializeStorageEngine();
            wireup.UsingBsonSerialization();
            return wireup.Build();
        }

        private static void BsonInit()
        {
            foreach (Type classType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(e => e.GetTypes().Where(f =>
            {
                if (!f.IsSubclassOf(typeof(EventBase)))
                    return f.IsSubclassOf(typeof(Aggregate));
                return true;
            })))
                BsonClassMap.LookupClassMap(classType);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (Libs.ContainsKey(args.Name))
                return Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Libs[args.Name]));
            return Assembly.LoadFrom("");
        }
    }
}
