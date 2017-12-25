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
            var castle = snapshot?.Castles?.FirstOrDefault(e => e.Id == data.EventObject.CastleId);
            if (castle == null)
                return false;
            if (castle.Soldiers == null)
                castle.Soldiers = new List<SoldierAggregate>();
            ++castle.SoldiersAmount;
            SoldierAggregate soldierAggregate = new SoldierAggregate { Id = Guid.NewGuid() };
            var castleTroopType = castle.GetTroopTypeData(data.EventObject.TroopType);
            if (castleTroopType != null)
            {
                soldierAggregate.CastleTroopType = castleTroopType;
                castle.Soldiers.Add(soldierAggregate);
                castle.UpdateSoldierAmount();
            }
            _gameDomainService.CreateSoldierIfNeed(snapshot, castle);
            return true;
        }
    }
}
