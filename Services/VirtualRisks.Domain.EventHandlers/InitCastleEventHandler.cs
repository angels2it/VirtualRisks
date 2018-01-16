using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class InitCastleEventHandler : IDomainEventHandler<InitCastleEvent>
    {
        public bool Handle(DomainEventHandlerData<InitCastleEvent> @event)
        {
            GameAggregate snapshot = @event.Snapshot as GameAggregate;
            var castle = snapshot?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.Id);
            if (castle == null)
                return false;
            castle.Name = @event.EventObject.Name;
            castle.Army = @event.EventObject.Army;
            castle.Position = @event.EventObject.Position;
            castle.MaxResourceLimit = @event.EventObject.MaxResourceLimit;
            castle.OwnerUserId = @event.EventObject.OwnerUserId;
            castle.OwnerId = @event.EventObject.OwnerId;
            castle.Soldiers = new List<SoldierAggregate>();
            castle.Strength = @event.EventObject.Strength;
            return true;
        }
    }
}
