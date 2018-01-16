using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class RestartCastleProductionEventHandler : IDomainEventHandler<RestartCastleProductionEvent>
    {
        private readonly IGameDomainService _gameDomainService;

        public RestartCastleProductionEventHandler(IGameDomainService gameDomainService)
        {
            _gameDomainService = gameDomainService;
        }

        public bool Handle(DomainEventHandlerData<RestartCastleProductionEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            //var troop = castle.GetDefaultTroopType();
            //var needCoins = _gameDomainService.GetUpkeepCoinBySoldierType(castle, troop);
            //if (snap.CanProduce(castle, needCoins))
            //    castle.RestartProduction();
            return true;
        }
    }
}