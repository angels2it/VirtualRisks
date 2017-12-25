using CastleGo.Domain.Interfaces;
using NEventStore;
using System;

namespace CastleGo.Domain.Bases
{
    public class DomainEventHandlerData<T> where T : IEventBase
    {
        public object Event { get; set; }

        public T EventObject => (T)this.Event;

        public Aggregate Snapshot { get; set; }

        public IStoreEvents Store { get; set; }

        public Guid Id { get; set; }

        public static object CreateDynamicInstance(Type type, DomainEventHandlerData<EventBase> data)
        {
            dynamic a = Activator.CreateInstance(type);
            a.Event = data.Event;
            a.Snapshot = data.Snapshot;
            a.Store = data.Store;
            a.Id = data.Id;
            return a;
        }
    }
}
