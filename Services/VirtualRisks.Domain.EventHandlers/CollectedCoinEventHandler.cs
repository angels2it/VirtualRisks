using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class CollectedCoinEventHandler : IDomainEventHandler<CollectedCoinEvent>
    {
        public bool Handle(DomainEventHandlerData<CollectedCoinEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            if (snap == null)
                return false;
            switch (@event.EventObject.Army)
            {
                case Army.Red:
                    snap.OpponentCoins += @event.EventObject.Coins;
                    break;
                case Army.Blue:
                    snap.UserCoins += @event.EventObject.Coins;
                    break;
            }
            return true;
        }
    }
}