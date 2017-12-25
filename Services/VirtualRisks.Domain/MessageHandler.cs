// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.MessageHandler
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using Autofac;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CastleGo.Domain
{
    public static class MessageHandler
    {
        private static readonly Dictionary<Type, Action<MessageBase>> Routes = new Dictionary<Type, Action<MessageBase>>();
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
                .Where(mi => mi.Name == "RegisterMessageHandler")
                .Where(mi => mi.IsGenericMethod)
                .Where(mi => mi.GetGenericArguments().Count() == 1)
                .Single(mi => mi.GetParameters().Count() == 1)
                .MakeGenericMethod(commandType);

            var del = new Action<dynamic>(x =>
            {
                dynamic handler = _container.Resolve(@interface);
                handler.Handle(x);
            });

            registerExecutorMethod.Invoke(bus, new object[] { del });
        }

        private static IEnumerable<Type> ResolveMessageHandlerInterface(Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Where(i =>
            {
                if (i.IsConstructedGenericType)
                    return i.GetGenericTypeDefinition() == typeof(IMessageHandler<>);
                return false;
            });
        }

        public static void Add<T>(Action<T> handler) where T : MessageBase
        {
            Action<MessageBase> action;
            if (Routes.TryGetValue(typeof(T), out action))
                return;
            Routes.Add(typeof(T), (Action<MessageBase>)handler);
        }

        public static void Publish<T>(T @event) where T : MessageBase
        {
            Action<MessageBase> action;
            if (!Routes.TryGetValue(@event.GetType(), out action))
                return;
            action.DynamicInvoke((object)@event);
        }
    }
}
