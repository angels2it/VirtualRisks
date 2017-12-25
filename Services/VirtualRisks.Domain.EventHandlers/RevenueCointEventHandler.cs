using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class RevenueCointEventHandler : IDomainEventHandler<RevenueCointEvent>
    {
        private readonly IGameDomainService _gameDomainService;
        private readonly IDomainService _domainService;

        public RevenueCointEventHandler(IGameDomainService gameDomainService, IDomainService domainService)
        {
            _gameDomainService = gameDomainService;
            _domainService = domainService;
        }

        public bool Handle(DomainEventHandlerData<RevenueCointEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            if (snap == null)
                return false;
            var relateEvents = new List<EventBase>();
            var userCastles = snap.Castles.Where(e => e.OwnerUserId == snap.UserId).ToList() ?? new List<CastleAggregate>();
            foreach (var castle in userCastles)
            {
                var coins = _gameDomainService.CalculateCoin(snap, castle);
                snap.UserCoins += coins;
                relateEvents.Add(new CastleRevenueCointEvent(castle.Id, coins, snap.UserId, DateTime.UtcNow, DateTime.UtcNow));
                var createSoldier = _gameDomainService.GetCreateSoldierIfNeedCreate(snap, castle);
                if (createSoldier != null)
                    relateEvents.Add(createSoldier);
            }

            var opponentCastles = snap.Castles.Where(e => e.OwnerUserId == snap.OpponentId).ToList() ?? new List<CastleAggregate>();
            foreach (var castle in opponentCastles)
            {
                var coins = _gameDomainService.CalculateCoin(snap, castle);
                snap.OpponentCoins += coins;
                relateEvents.Add(new CastleRevenueCointEvent(castle.Id, coins, snap.OpponentId, DateTime.UtcNow, DateTime.UtcNow));
                var createSoldier = _gameDomainService.GetCreateSoldierIfNeedCreate(snap, castle);
                if (createSoldier != null)
                    relateEvents.Add(createSoldier);

            }
            relateEvents.Add(_gameDomainService.RevenueCoinEvent(snap.Speed));
            _domainService.AddEvent(snap.Id, relateEvents.ToArray());
            return true;
        }
    }
}