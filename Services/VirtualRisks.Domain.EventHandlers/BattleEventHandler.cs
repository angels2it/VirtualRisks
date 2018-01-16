using System;
using System.Linq;
using CastleGo.DataAccess;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;
using CastleGo.Entities;
using MongoRepository;

namespace CastleGo.Domain.EventHandlers
{
    public class BattleEventHandler : IDomainEventHandler<BattleEvent>
    {
        private readonly IDomainService _domain;
        private readonly IRepository<User> _repository;

        private readonly IGameDomainService _gameDomainService;

        public BattleEventHandler(IDomainService domain, IGameDomainService gameDomainService, IRepository<User> repository)
        {
            _domain = domain;
            _gameDomainService = gameDomainService;
            _repository = repository;
        }

        public bool Handle(DomainEventHandlerData<BattleEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.AtCastleId);
            if (castle?.Siege?.Soldiers == null)
                return false;
            var attacking = castle.Siege.Soldiers;
            var defending = castle.Soldiers.Where(e => !e.IsDead).ToList();
            // boost strength
            var hero = castle.Heroes?.FirstOrDefault(e => e.Army != castle.Army);
            var attackingBoostStrength = hero == null ? 0 : _repository.GetHeroById(hero.UserId, hero.Id.ToString())?.Leadership ?? 0;

            hero = castle.Heroes?.FirstOrDefault(e => e.Army == castle.Army);
            var defendingBoostStrength = hero == null ? 0 : _repository.GetHeroById(hero.UserId, hero.Id.ToString())?.Leadership ?? 0;

            var battle = _gameDomainService.AttackSequence(@event.EventObject.Id, 
                castle,
                castle.Siege.OwnerUserId,
                castle.OwnerUserId,
                 attacking,
                 defending,
                attackingBooststrength: attackingBoostStrength,
                defendingBooststrength: defendingBoostStrength);
            // attacking win
            if (attacking.Count(e => !e.IsDead && e.CastleTroopType.Health > 0) > 0)
            {
                // add event
                // for user
                _domain.AddEvent(snap.Id, new OccupiedCastleEvent(castle.Siege.OwnerUserId, castle.Id, battle, DateTime.UtcNow));
                // for old owner of castle
                if (!string.IsNullOrEmpty(castle.OwnerUserId))
                    _domain.AddEvent(snap.Id, new CastleHasBeenOccupiedEvent(castle.OwnerUserId, castle.Id, battle, DateTime.UtcNow));
            }
            else
            {
                // defending win
                castle.Soldiers = castle.Soldiers.Where(e => e.CastleTroopType.Health > 0 && !e.IsDead).ToList();
                castle.UpdateSoldierAmount();
                _gameDomainService.CreateSoldierIfNeed(snap);
                // for depending
                if (!string.IsNullOrEmpty(castle.OwnerUserId))
                    _domain.AddEvent(snap.Id, new DefendedCastleEvent(castle.OwnerUserId, castle.Id, battle, DateTime.UtcNow));
                // for attacking
                _domain.AddEvent(snap.Id, new FailedAttackCastleEvent(castle.Siege.OwnerUserId, castle.Id, battle, DateTime.UtcNow));
            }
            return true;
        }
    }
}
