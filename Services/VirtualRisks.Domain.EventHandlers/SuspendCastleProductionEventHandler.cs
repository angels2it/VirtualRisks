using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class SuspendCastleProductionEventHandler : IDomainEventHandler<SuspendCastleProductionEvent>
    {
        public bool Handle(DomainEventHandlerData<SuspendCastleProductionEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            castle.SuspendProduction();
            return true;
        }
    }
}