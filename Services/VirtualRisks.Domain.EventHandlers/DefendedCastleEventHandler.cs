using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;

namespace CastleGo.Domain.EventHandlers
{
    public class DefendedCastleEventHandler : IDomainEventHandler<DefendedCastleEvent>
    {

        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;

        public DefendedCastleEventHandler(IDomainService domain, GameSettings gameSettings)
        {
            _domain = domain;
            _gameSettings = gameSettings;
        }

        public bool Handle(DomainEventHandlerData<DefendedCastleEvent> @event)
        {
            return true;
        }
    }
}
