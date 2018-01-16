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
    public class CreateSoldierEventHandler : IDomainEventHandler<CreateSoldierEvent>
    {
        private readonly IDomainService _domain;
        private readonly IGameDomainService _gameDomainService;

        public CreateSoldierEventHandler(IDomainService domainService, IGameDomainService gameDomainService)
        {
            _domain = domainService;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<CreateSoldierEvent> data)
        {
            GameAggregate snapshot = data.Snapshot as GameAggregate;
            if (snapshot == null)
                return false;
            if (data.EventObject.Army == Army.Blue)
            {
                if (snapshot.UserSoldiers == null)
                    snapshot.UserSoldiers = new List<SoldierAggregate>();
                ++snapshot.UserSoldiersAmount;
            }
            else
            {
                if (snapshot.OpponentSoldiers == null)
                    snapshot.OpponentSoldiers = new List<SoldierAggregate>();
                ++snapshot.OpponentSoldierAmount;
            }
            SoldierAggregate soldierAggregate = new SoldierAggregate { Id = Guid.NewGuid() };
            var castleTroopType = snapshot.GetTroopTypeData(data.EventObject.Army, data.EventObject.TroopType);
            if (castleTroopType != null)
            {
                soldierAggregate.CastleTroopType = castleTroopType;
                if (data.EventObject.Army == Army.Blue)
                    snapshot.UserSoldiers.Add(soldierAggregate);
                else
                    snapshot.OpponentSoldiers.Add(soldierAggregate);
            }
            _gameDomainService.CreateSoldierIfNeed(snapshot, data.EventObject.Army);
            return true;
        }
    }
}
