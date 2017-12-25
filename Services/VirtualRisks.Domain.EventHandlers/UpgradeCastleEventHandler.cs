using System.Linq;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;

namespace CastleGo.Domain.EventHandlers
{
    public class UpgradeCastleEventHandler : IDomainEventHandler<UpgradeCastleEvent>
    {
        private readonly IGameDomainService _domainService;
        private readonly IDomainService _domain;

        public UpgradeCastleEventHandler(IGameDomainService domainService, IDomainService domain)
        {
            _domainService = domainService;
            _domain = domain;
        }

        public bool Handle(DomainEventHandlerData<UpgradeCastleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle == null)
                return false;
            castle.Strength++;
            var coins = _domainService.GetCoinsToUpgradeCastle(castle.Strength);
            _domain.AddEvent(snap.Id, new DeficitCoinsEvent(coins, castle.Army, castle.OwnerUserId));
            return true;
        }
    }
}
