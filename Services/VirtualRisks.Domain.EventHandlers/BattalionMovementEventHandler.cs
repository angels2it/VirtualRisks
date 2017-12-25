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
    public class ChangeCastleTroopTypeEventHandler : IDomainEventHandler<ChangeCastleTroopTypeEvent>
    {
        public bool Handle(DomainEventHandlerData<ChangeCastleTroopTypeEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            castle.ProducedTroopTypes = new List<string>()
            {
                @event.EventObject.TroopType
            };
            return true;
        }
    }
    public class BattalionMovementEventHandler : IDomainEventHandler<BattalionMovementEvent>
    {
        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;
        private readonly IGameDomainService _gameDomainService;

        public BattalionMovementEventHandler(IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService)
        {
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<BattalionMovementEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            var destination = snap.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.DestinationCastleId);
            if (destination == null)
                return false;
            if (castle.Army == destination.Army)
            {
                // just add soldier
                if (destination.Soldiers == null)
                    destination.Soldiers = new List<SoldierAggregate>();
                var soldierCanAdd = destination.MaxResourceLimit - destination.Soldiers.Count;
                if (soldierCanAdd > 0)
                {
                    var canAddSoldiers = @event.EventObject.Soldiers
                        .Where(e => destination.Soldiers.All(f => f.Id != e.Id))
                        .OrderByDescending(e => e.CastleTroopType.Health)
                        .Take(
                            soldierCanAdd);
                    destination.Soldiers.AddRange(canAddSoldiers);
                    destination.UpdateSoldierAmount();
                }
                if (@event.EventObject.Soldiers.Count > soldierCanAdd)
                {
                    //todo: fill all soldier still in battalion to closest castle
                }
                return true;
            }
            // check siege of destination castle
            if (destination.Siege == null)
            {
                _gameDomainService.AddSiegeEvent(snap, castle, destination, @event.EventObject.Soldiers);
            }
            else if (destination.Siege.OwnerUserId != castle.OwnerUserId)
            {
                // start battle
                _domain.AddEvent(snap.Id,
                    new BattleVersusSiegeEvent(
                        castle.OwnerUserId,
                        destination.Id,
                        @event.EventObject.Soldiers,
                        DateTime.UtcNow));
            }
            else
            {
                // add more soldier to siege
                if (destination.Siege.Soldiers == null)
                    destination.Siege.Soldiers = new List<SoldierAggregate>();
                var canAddSoldiers = @event.EventObject.Soldiers.Where(e => destination.Siege.Soldiers.All(f => f.Id != e.Id)).ToList();
                destination.Siege.Soldiers.AddRange(canAddSoldiers);
                castle.Soldiers.RemoveAll(e => canAddSoldiers.Any(f => f.Id == e.Id));
                destination.UpdateSoldierAmount();
                castle.UpdateSoldierAmount();
            }
            return true;
        }
    }
}
