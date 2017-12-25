using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class CastleUpkeepCointEventHandler : IDomainEventHandler<CastleUpkeepCointEvent>
    {
        public bool Handle(DomainEventHandlerData<CastleUpkeepCointEvent> @event)
        {
            return true;
        }
    }
}