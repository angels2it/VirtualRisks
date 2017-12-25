using System;
using System.Linq;
using CastleGo.DataAccess;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;
using CastleGo.Entities;
using CastleGo.Shared.Common;
using MongoRepository;

namespace CastleGo.Domain.EventHandlers
{
    public class BattleVersusSiegeEventHandler : IDomainEventHandler<BattleVersusSiegeEvent>
    {
        private readonly IDomainService _domain;
        private readonly IGameDomainService _gameDomainService;
        private readonly IRepository<User> _repository;

        public BattleVersusSiegeEventHandler(IDomainService domain, IGameDomainService gameDomainService, IRepository<User> repository)
        {
            _domain = domain;
            _gameDomainService = gameDomainService;
            _repository = repository;
        }

        public bool Handle(DomainEventHandlerData<BattleVersusSiegeEvent> @event)
        {
            var snap = @event.Snapshot as GameAggregate;
            var castle = snap?.Castles.FirstOrDefault(e => e.Id == @event.EventObject.CastleId);
            if (castle?.Siege.Soldiers == null)
                return false;
            var attacking = @event.EventObject.Battalion;
            var defending = castle.Siege.Soldiers;

            // boost strength
            var attackingArmy = snap.UserId == @event.EventObject.AttackingUserId ? Army.Blue : Army.Red;
            var hero = castle.Heroes?.FirstOrDefault(e => e.Army == attackingArmy);
            var attackingBoostStrength = hero == null ? 0 : _repository.GetHeroById(hero.UserId, hero.Id.ToString())?.Leadership ?? 0;

            var defendingArmy = snap.UserId == castle.Siege.OwnerUserId ? Army.Blue : Army.Red;
            hero = castle.Heroes?.FirstOrDefault(e => e.Army == defendingArmy);
            var defendingBoostStrength = hero == null ? 0 : _repository.GetHeroById(hero.UserId, hero.Id.ToString())?.Leadership ?? 0;

            var battle = _gameDomainService.AttackSequence(@event.EventObject.Id,
                castle,
                @event.EventObject.AttackingUserId,
                castle.Siege.OwnerUserId,
                 attacking,
                 defending,
                attackingBoostStrength,
                defendingBoostStrength);
            // attacking win
            if (attacking.Count(e => !e.IsDead && e.CastleTroopType.Health > 0) > 0)
            {
                var liveSoldiers = attacking.Where(e => e.CastleTroopType.Health > 0).ToList();
                // event for attacking
                _domain.AddEvent(snap.Id, new OccupiedSiegeInCastleEvent(
                    @event.EventObject.AttackingUserId,
                    castle.Id,
                    battle,
                    liveSoldiers, DateTime.UtcNow));
                // for old siege
                _domain.AddEvent(snap.Id, new SiegeHasBeenOccupiedEvent(castle.Siege.OwnerUserId,
                    castle.Id,
                    battle,
                    DateTime.UtcNow));
            }
            else
            {
                // defending win
                // for defending
                _domain.AddEvent(snap.Id, new DefendedSiegeEvent(
                    castle.Id, battle, castle.Siege.OwnerUserId));
                // for attacking
                _domain.AddEvent(snap.Id, new FailedAttackSiegeEvent(
                    @event.EventObject.AttackingUserId,
                    castle.Id,
                    battle));
            }
            return true;
        }
    }
}
