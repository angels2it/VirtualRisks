using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class ChangeGameStatusEventHandler : IDomainEventHandler<ChangeGameStatusEvent>
    {
        public bool Handle(DomainEventHandlerData<ChangeGameStatusEvent> @event)
        {
            GameAggregate snapshot = @event.Snapshot as GameAggregate;
            if (snapshot == null)
                return false;
            GameAggregate gameAggregate = snapshot;
            ChangeGameStatusEvent eventObject = @event.EventObject;
            int num = eventObject != null ? (int)eventObject.Status : 0;
            gameAggregate.Status = (GameStatus)num;
            return true;
        }
    }
}
