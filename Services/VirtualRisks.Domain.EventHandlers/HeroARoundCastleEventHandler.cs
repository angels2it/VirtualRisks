using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class HeroARoundCastleEventHandler : IDomainEventHandler<HeroARoundCastleEvent>
    {
        public bool Handle(DomainEventHandlerData<HeroARoundCastleEvent> @event)
        {
            var game = @event.Snapshot as GameAggregate;
            var castle = game?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            if (castle.Heroes == null)
                castle.Heroes = new List<HeroAggregate>();
            castle.Heroes.RemoveAll(e => e.Id == new Guid(@event.EventObject.HeroId));
            var army = game.UserId == @event.EventObject.UserId ? Army.Blue : Army.Red;
            castle.Heroes.Add(new HeroAggregate()
            {
                Id = new Guid(@event.EventObject.HeroId),
                UserId = @event.EventObject.UserId,
                Army = army,
                UpdatedAt = @event.EventObject.ExecuteAt
            });
            return true;
        }
    }
}
