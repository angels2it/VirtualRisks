using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class CastleHasBeenOccupiedEventHandler : IDomainEventHandler<CastleHasBeenOccupiedEvent>
    {
        public bool Handle(DomainEventHandlerData<CastleHasBeenOccupiedEvent> @event)
        {
            return true;
        }
    }
}
