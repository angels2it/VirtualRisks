// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.EventHandler
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using Autofac;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Interfaces;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CastleGo.Domain
{
    public static class EventHandler
    {
        private static readonly Dictionary<Type, Func<DomainEventHandlerData<IEventBase>, bool>> Routes = new Dictionary<Type, Func<DomainEventHandlerData<IEventBase>, bool>>();
        private static IContainer _container;

        public static void Init(IContainer container)
        {
            _container = container;
            foreach (var data in AppDomain.CurrentDomain.GetAssemblies().Where(e => !e.IsDynamic).SelectMany(e => e.ExportedTypes).Select(t => new { Type = t, Interfaces = ResolveMessageHandlerInterface(t) }).Where(e =>
          {
              if (e.Interfaces != null)
                  return e.Interfaces.Any();
              return false;
          }))
            {
                foreach (Type @interface in data.Interfaces)
                    InvokeHandler(@interface, new HandlerRegistrar(), data.Type);
            }
        }

        private static void InvokeHandler(Type @interface, IHandlerRegistrar bus, Type executorType)
        {
            var commandType = @interface.GenericTypeArguments[0];
            var registerExecutorMethod = bus
                .GetType().GetRuntimeMethods()
                .Where(mi => mi.Name == "RegisterHandler")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Count() == 1)
                .Single(mi => mi.GetParameters().Count() == 1)
                .MakeGenericMethod(commandType);

            var del = new Func<dynamic, bool>(x =>
            {
                dynamic handler = _container.Resolve(@interface);
                var d1 = typeof(DomainEventHandlerData<>);
                Type[] typeArgs = { x.Event.GetType() };
                var makeme = d1.MakeGenericType(typeArgs);
                var a = DomainEventHandlerData<EventBase>.CreateDynamicInstance(makeme, x);
                return handler.Handle(a);
            });

            registerExecutorMethod.Invoke(bus, new object[] { del });
        }

        private static IEnumerable<Type> ResolveMessageHandlerInterface(Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Where(i =>
            {
                if (i.IsConstructedGenericType)
                    return i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>);
                return false;
            });
        }

        public static void Add<T>(Func<DomainEventHandlerData<T>, bool> handler) where T : IEventBase
        {
            Func<DomainEventHandlerData<IEventBase>, bool> func;
            if (Routes.TryGetValue(typeof(T), out func))
                return;
            Routes.Add(typeof(T), (Func<DomainEventHandlerData<IEventBase>, bool>)handler);
        }

        public static bool Publish<T>(DomainEventHandlerData<T> @event) where T : EventBase
        {
            Func<DomainEventHandlerData<IEventBase>, bool> func;
            if (!Routes.TryGetValue(@event.Event.GetType(), out func))
                return false;
            return (bool)func.DynamicInvoke((object)@event);
        }
    }
}
