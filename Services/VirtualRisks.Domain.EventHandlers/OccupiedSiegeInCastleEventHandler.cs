using System;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class OccupiedSiegeInCastleEventHandler : IDomainEventHandler<OccupiedSiegeInCastleEvent>
    {
        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;
        private readonly IGameDomainService _gameDomainService;
        public OccupiedSiegeInCastleEventHandler(IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService)
        {
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<OccupiedSiegeInCastleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            castle.Siege = new SiegeAggregate()
            {
                Id = Guid.NewGuid(),
                OwnerUserId = @event.EventObject.CreatedBy,
                Soldiers = @event.EventObject.Soldiers,
                BattleAt = _gameDomainService.GetBattleTime(snap.Speed)
            };
            // add event
            _gameDomainService.AddSiegeEvent(snap, castle, @event.EventObject.Soldiers);
            return true;
        }
    }
}
