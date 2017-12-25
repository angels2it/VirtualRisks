using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class FailedAttackSiegeEventHandler : IDomainEventHandler<FailedAttackSiegeEvent>
    {
        public bool Handle(DomainEventHandlerData<FailedAttackSiegeEvent> @event)
        {
            return true;
        }
    }
}
