using System;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class DefendedSiegeEventHandler : IDomainEventHandler<DefendedSiegeEvent>
    {
        private readonly IDomainService _domain;
        private readonly IGameDomainService _gameDomainService;
        private readonly GameSettings _gameSettings;

        public DefendedSiegeEventHandler(IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService)
        {
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<DefendedSiegeEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle?.Siege == null)
                return false;
            castle.Siege.Soldiers = castle.Siege.Soldiers.Where(e => e.CastleTroopType.Health > 0).ToList();
            _gameDomainService.AddSiegeEvent(snap, castle, castle.Siege.Soldiers);
            //_domain.AddEvent(snap.Id, new SiegeCastleEvent(castle.Siege.OwnerUserId,
            //    castle.Id,
            //    castle.Siege.Soldiers,
            //    DateTime.UtcNow,
            //    DateTime.UtcNow.AddMinutes(_gameSettings.SiegeTime)));
            return true;
        }
    }
}
