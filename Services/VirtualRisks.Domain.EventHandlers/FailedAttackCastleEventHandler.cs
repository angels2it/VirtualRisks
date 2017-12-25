using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class FailedAttackCastleEventHandler : IDomainEventHandler<FailedAttackCastleEvent>
    {
        public bool Handle(DomainEventHandlerData<FailedAttackCastleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            castle.Siege = null;
            return true;
        }
    }
}