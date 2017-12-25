using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class CastleRevenueCointEventHandler : IDomainEventHandler<CastleRevenueCointEvent>
    {
        public bool Handle(DomainEventHandlerData<CastleRevenueCointEvent> @event)
        {
            return true;
        }
    }
}
