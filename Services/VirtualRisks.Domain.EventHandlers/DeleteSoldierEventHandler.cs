// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Events.Handlers.DeleteSoldierEventHandler

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using TinyMessenger;

namespace CastleGo.Domain.EventHandlers
{
    public class DeleteSoldierEventHandler : IDomainEventHandler<DeleteSoldierEvent>
    {
        private readonly ITinyMessengerHub _hub;

        public DeleteSoldierEventHandler(ITinyMessengerHub hub)
        {
            _hub = hub;
        }

        public bool Handle(DomainEventHandlerData<DeleteSoldierEvent> data)
        {
            CastleAggregate snapshot = data.Snapshot as CastleAggregate;
            if (snapshot != null)
            {
                Debug.WriteLine("Soldier deleted!");
                --snapshot.SoldiersAmount;
                List<SoldierAggregate> soldiers = snapshot.Soldiers;
                SoldierAggregate soldierAggregate1;
                if (soldiers == null)
                {
                    soldierAggregate1 = null;
                }
                else
                {
                    Func<SoldierAggregate, bool> predicate = e => e.Id == data.EventObject.SoldierId;
                    soldierAggregate1 = soldiers.FirstOrDefault<SoldierAggregate>(predicate);
                }
                SoldierAggregate soldierAggregate2 = soldierAggregate1;
                if (soldierAggregate2 != null)
                    soldierAggregate2.IsDead = true;
            }
            return true;
        }
    }
}
