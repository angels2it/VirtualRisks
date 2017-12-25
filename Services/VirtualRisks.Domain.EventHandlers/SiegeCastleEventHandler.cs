using System;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class SiegeCastleEventHandler : IDomainEventHandler<SiegeCastleEvent>
    {
        private readonly IDomainService _domain;

        public SiegeCastleEventHandler(IDomainService domain)
        {
            _domain = domain;
        }

        public bool Handle(DomainEventHandlerData<SiegeCastleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            if (snap == null)
                return false;
            _domain.AddEvent(snap.Id, new BattleEvent(@event.EventObject.CastleId, DateTime.UtcNow, DateTime.UtcNow));
            return true;
        }
    }
}
