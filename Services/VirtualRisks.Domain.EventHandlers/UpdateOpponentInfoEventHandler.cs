using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class UpdateOpponentInfoEventHandler : IDomainEventHandler<UpdateOpponentInfoEvent>
    {
        public bool Handle(DomainEventHandlerData<UpdateOpponentInfoEvent> @event)
        {
            var game = @event.Snapshot as GameAggregate;
            if (game == null)
                return false;
            game.OpponentId = @event.EventObject.OpponentId;
            game.OpponentHeroId = @event.EventObject.OpponentHeroId;
            return true;
        }
    }
}