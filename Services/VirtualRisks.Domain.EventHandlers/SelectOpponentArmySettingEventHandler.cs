using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class SelectOpponentArmySettingEventHandler : IDomainEventHandler<SelectOpponentArmySettingEvent>
    {
        public bool Handle(DomainEventHandlerData<SelectOpponentArmySettingEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            if (snap == null)
                return false;
            snap.OpponentArmySetting = @event.EventObject.ArmySetting;
            return true;
        }
    }
}
