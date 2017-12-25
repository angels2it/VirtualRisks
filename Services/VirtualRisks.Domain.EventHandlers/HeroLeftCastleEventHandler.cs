using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class HeroLeftCastleEventHandler : IDomainEventHandler<HeroLeftCastleEvent>
    {
        public bool Handle(DomainEventHandlerData<HeroLeftCastleEvent> @event)
        {
            var game = @event.Snapshot as GameAggregate;
            var castle = game?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            if (castle.Heroes == null)
                castle.Heroes = new List<HeroAggregate>();
            castle.Heroes.RemoveAll(e => e.Id == new Guid(@event.EventObject.HeroId));
            return true;
        }
    }
}
