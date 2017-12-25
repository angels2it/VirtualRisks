using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class SiegeHasBeenOccupiedEventHandler : IDomainEventHandler<SiegeHasBeenOccupiedEvent>
    {
        public bool Handle(DomainEventHandlerData<SiegeHasBeenOccupiedEvent> @event)
        {
            return true;
        }
    }
}
