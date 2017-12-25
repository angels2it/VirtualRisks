using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.EventHandlers
{
    public class EndGameEventHandler : IDomainEventHandler<EndGameEvent>
    {
        public bool Handle(DomainEventHandlerData<EndGameEvent> @event)
        {
            var game = @event.Snapshot as GameAggregate;
            if (game == null)
                return false;
            game.Status = GameStatus.Ended;
            game.Winner = @event.EventObject.Winner;
            return true;
        }
    }
}
