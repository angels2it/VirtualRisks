using System;
using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class StartBattalionEventHandler : IDomainEventHandler<StartBattalionEvent>
    {
        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;
        private readonly IGameDomainService _gameDomainService;

        public StartBattalionEventHandler(IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService)
        {
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<StartBattalionEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles?.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            var movedSoldiers = castle.Soldiers.Where(s => @event.EventObject.Soldiers.Any(e => new Guid(e) == s.Id)).ToList();
            castle.Soldiers.RemoveAll(s => @event.EventObject.Soldiers.Any(e => new Guid(e) == s.Id));
            castle.UpdateSoldierAmount();
            var e2 = new BattalionMovementEvent(castle.Id,
                @event.EventObject.DestinationCastleId,
                movedSoldiers,
                @event.EventObject.Route,
                @event.EventObject.CreatedBy,
                DateTime.UtcNow, DateTime.UtcNow.Add(@event.EventObject.Route.Duration));
            e2.Id = @event.EventObject.MovementId;
            _domain.AddEvent(snap.Id, e2);
            _gameDomainService.CreateSoldierIfNeed(snap);
            return true;
        }
    }
}