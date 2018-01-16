using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class InitGameEventHandler : IDomainEventHandler<InitGameEvent>
    {
        public bool Handle(DomainEventHandlerData<InitGameEvent> @event)
        {
            var game = @event.Snapshot as GameAggregate;
            if (game == null)
                return false;
            game.UserId = @event.EventObject.UserId;
            game.UserHeroId = @event.EventObject.UserHeroId;
            game.OpponentId = @event.EventObject.OpponentId;
            game.OpponentHeroId = @event.EventObject.OpponentHeroId;
            game.SelfPlaying = @event.EventObject.SelfPlaying;
            game.Speed = @event.EventObject.Speed;
            game.Difficulty = @event.EventObject.Difficulty;
            game.UserArmySetting = @event.EventObject.UserArmySetting;

            game.UserProducedTroopTypes = @event.EventObject.UserProducedTroopTypes;
            game.UserTroopTypes = @event.EventObject.UserTroopTypes;
            game.OpponentTroopTypes = @event.EventObject.OpponentTroopTypes;
            game.OpponentProducedTroopTypes = @event.EventObject.OpponentProducedTroopTypes;
            return true;
        }
    }
}
