using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class AddCastleEventHandler : IDomainEventHandler<AddCastleEvent>
    {
        public bool Handle(DomainEventHandlerData<AddCastleEvent> @event)
        {
            GameAggregate snapshot = @event.Snapshot as GameAggregate;
            if (snapshot == null)
                return false;
            if (snapshot.Castles == null)
                snapshot.Castles = new List<CastleAggregate>();
            snapshot.Castles.Add(new CastleAggregate()
            {
                Id = @event.EventObject.Id
            });
            return true;
        }
    }
}
