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
    public class UpkeepCoinEventHandler : IDomainEventHandler<UpkeepCoinEvent>
    {
        private readonly IGameDomainService _gameDomainService;
        private readonly IDomainService _domainService;

        public UpkeepCoinEventHandler(IGameDomainService gameDomainService, IDomainService domainService)
        {
            _gameDomainService = gameDomainService;
            _domainService = domainService;
        }

        public bool Handle(DomainEventHandlerData<UpkeepCoinEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            if (snap == null)
                return false;
            var relateEvents = new List<EventBase>();
            var userCastles = snap.Castles.Where(e => e.OwnerUserId == snap.UserId).ToList() ?? new List<CastleAggregate>();
            foreach (var castle in userCastles)
            {
                var coins = _gameDomainService.CalculateUpkeepCoin(snap.Id, castle);
                snap.UserCoins -= coins;
                relateEvents.Add(new CastleUpkeepCointEvent(castle.Id, coins, snap.UserId, DateTime.UtcNow, DateTime.UtcNow));
                //var createSoldier = _gameDomainService.GetCreateSoldierIfNeedCreate(snap, castle);
                //if (createSoldier != null)
                //    relateEvents.Add(createSoldier);
                //var isCreated = _gameDomainService.CreateSoldierIfNeed(snap, castle);
                //if (!isCreated && castle.Army != Army.Neutrual && (castle.Army == Army.Blue || !snap.SelfPlaying))
                //{
                //    var troopType = castle.GetDefaultTroopType();
                //    var needCoin = _gameDomainService.GetUpkeepCoinBySoldierType(castle, troopType);
                //    coins = castle.Army == Army.Blue ? snap.UserCoins : snap.OpponentCoins;
                //    if (needCoin > coins && castle.IsProductionState())
                //    {
                //        relateEvents.Add(new SuspendCastleProductionEvent(castle.Id, castle.OwnerUserId));
                //    }
                //}
            }

            var opponentCastles = snap.Castles.Where(e => e.OwnerUserId == snap.OpponentId).ToList() ?? new List<CastleAggregate>();
            foreach (var castle in opponentCastles)
            {
                var coins = _gameDomainService.CalculateUpkeepCoin(snap.Id, castle);
                snap.OpponentCoins -= coins;
                relateEvents.Add(new CastleUpkeepCointEvent(castle.Id, coins, snap.OpponentId, DateTime.UtcNow, DateTime.UtcNow));
                //var createSoldier = _gameDomainService.GetCreateSoldierIfNeedCreate(snap, castle);
                //if (createSoldier != null)
                //    relateEvents.Add(createSoldier);
                //var isCreated = _gameDomainService.CreateSoldierIfNeed(snap, castle);
                //if (!isCreated && castle.Army != Army.Neutrual && (castle.Army == Army.Blue || !snap.SelfPlaying))
                //{
                //    var troopType = castle.GetDefaultTroopType();
                //    var needCoin = _gameDomainService.GetUpkeepCoinBySoldierType(castle, troopType);
                //    coins = castle.Army == Army.Blue ? snap.UserCoins : snap.OpponentCoins;
                //    if (needCoin > coins && castle.IsProductionState())
                //    {
                //        relateEvents.Add(new SuspendCastleProductionEvent(castle.Id, castle.OwnerUserId));
                //    }
                //}
            }

            relateEvents.Add(_gameDomainService.UpkeepCoinEvent(snap.Speed));
            _domainService.AddEvent(snap.Id, relateEvents.ToArray());
            return true;
        }
    }
}
