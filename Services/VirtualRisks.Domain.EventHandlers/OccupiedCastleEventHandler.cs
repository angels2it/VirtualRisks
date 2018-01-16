using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class OccupiedCastleEventHandler : IDomainEventHandler<OccupiedCastleEvent>
    {
        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;
        private readonly IGameDomainService _gameDomainService;

        public OccupiedCastleEventHandler(IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService)
        {
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<OccupiedCastleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle?.Siege == null)
                return false;
            var liveSoldiers = castle.Siege.Soldiers?.Where(e => e.CastleTroopType.Health > 0).ToList() ?? new List<SoldierAggregate>();
            castle.Soldiers = liveSoldiers;
            castle.UpdateSoldierAmount();
            var army = snap.UserId == castle.Siege.OwnerUserId ? Army.Blue : Army.Red;
            castle.Siege = null;
            castle.Army = army;
            if (army == Army.Blue)
            {
                castle.OwnerId = snap.UserHeroId;
                castle.OwnerUserId = snap.UserId;
            }
            else
            {
                castle.OwnerId = snap.OpponentHeroId;
                castle.OwnerUserId = snap.OpponentId;
            }
            // check end game status
            bool isBlueWin = snap.Castles.All(e => e.Army == Army.Blue);
            var isRedWin = snap.Castles.All(e => e.Army == Army.Red);
            if (isBlueWin || isRedWin)
            {
                _domain.AddEvent(snap.Id, new EndGameEvent(isBlueWin ? Army.Blue : Army.Red, DateTime.UtcNow));
                return true;
            }
            _gameDomainService.CreateSoldierIfNeed(snap);
            return true;
        }
    }
}
