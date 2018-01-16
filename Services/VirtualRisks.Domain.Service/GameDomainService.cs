using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CastleGo.Common.Modules;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Events;
using CastleGo.Domain.Interfaces;
using CastleGo.Entities;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Service
{
    public class GameDomainService : IGameDomainService
    {
        private readonly IDomainService _domain;
        private readonly GameSettings _gameSettings;
        static readonly Random R = new Random();
        private readonly ISettingsReader _settingsReader;

        public GameDomainService(IDomainService domainService, GameSettings gameSettings, ISettingsReader settingsReader)
        {
            _domain = domainService;
            _gameSettings = gameSettings;
            _settingsReader = settingsReader;
        }
        public BattleLogAggregate AttackSequence(Guid id, CastleAggregate atCastle, string attackingId, string defendingId, List<SoldierAggregate> attacking, List<SoldierAggregate> defending, int attackingBooststrength = 0, int defendingBooststrength = 0)
        {
            int dIndex = 0, aIndex = 0;
            var battle = new BattleLogAggregate
            {
                Id = id,
                // clone object
                Attacking = attacking.Select(e => new SoldierAggregate()
                {
                    Id = e.Id,
                    IsDead = e.IsDead,
                    CastleTroopType = e.CastleTroopType == null ? null : new CastleTroopType()
                    {
                        AttackStrength = e.CastleTroopType.AttackStrength,
                        Health = e.CastleTroopType.Health,
                        MovementSpeed = e.CastleTroopType.MovementSpeed,
                        ProductionSpeed = e.CastleTroopType.ProductionSpeed,
                        ResourceType = e.CastleTroopType.ResourceType,
                        Icon = e.CastleTroopType.Icon,
                        RedArmyIcon = e.CastleTroopType.RedArmyIcon,
                        BlueArmyIcon = e.CastleTroopType.BlueArmyIcon
                    }
                }).ToList(),
                Defending = defending.Select(e => new SoldierAggregate()
                {
                    Id = e.Id,
                    IsDead = e.IsDead,
                    CastleTroopType = e.CastleTroopType == null ? null : new CastleTroopType()
                    {
                        AttackStrength = e.CastleTroopType.AttackStrength,
                        Health = e.CastleTroopType.Health,
                        MovementSpeed = e.CastleTroopType.MovementSpeed,
                        ProductionSpeed = e.CastleTroopType.ProductionSpeed,
                        ResourceType = e.CastleTroopType.ResourceType,
                        Icon = e.CastleTroopType.Icon,
                        RedArmyIcon = e.CastleTroopType.RedArmyIcon,
                        BlueArmyIcon = e.CastleTroopType.BlueArmyIcon
                    }
                }).ToList(),
                Logs = new List<BattleAttackingLogAggregate>()
            };
            while (dIndex < defending.Count && aIndex < attacking.Count)
            {
                // defending attack attacking
                var hit = R.NextDouble();
                var wallStrength = attacking[aIndex].CastleTroopType.IsOverComeWalls ? 0 : atCastle.Strength;
                attacking[aIndex].CastleTroopType.Health -= Math.Round(hit * defending[dIndex].CastleTroopType.AttackStrength +
                    defendingBooststrength +
                    wallStrength, MidpointRounding.AwayFromZero);
                battle.Logs.Add(new BattleAttackingLogAggregate()
                {
                    OwnerUserId = defendingId,
                    AttackingSoldierId = defending[dIndex].Id,
                    DefendingSoldierId = attacking[aIndex].Id,
                    Hit = hit,
                    Booststrength = defendingBooststrength,
                    WallStrength = wallStrength
                });
                if (attacking[aIndex].CastleTroopType.Health <= 0)
                    aIndex++;
                if (aIndex >= attacking.Count)
                    break;
                // attacking attack defending
                hit = R.NextDouble();
                defending[dIndex].CastleTroopType.Health -= Math.Round(hit * attacking[aIndex].CastleTroopType.AttackStrength + attackingBooststrength, MidpointRounding.AwayFromZero);
                battle.Logs.Add(new BattleAttackingLogAggregate()
                {
                    OwnerUserId = attackingId,
                    AttackingSoldierId = attacking[aIndex].Id,
                    DefendingSoldierId = defending[dIndex].Id,
                    Hit = hit,
                    Booststrength = attackingBooststrength
                });
                if (defending[dIndex].CastleTroopType.Health <= 0)
                    dIndex++;
            }
            return battle;
        }

        public CreateSoldierEvent GetCreateSoldierEvent(Guid gameId, Army army, string troopType, string ownerUserId, bool immediately = false)
        {
            var snap = _domain.GetGameSnapshot(gameId);
            var game = snap?.Payload as GameAggregate;
            double productionTime = 0;
            if (!immediately)
            {
                productionTime = _gameSettings.ProductionTime;
                if (game != null)
                    productionTime = productionTime * GameSpeedHelper.GetSpeedValue(game.Speed);
            }
            var @event = new CreateSoldierEvent(army,
                    troopType, DateTime.UtcNow,
                    TimeSpan.FromMinutes(productionTime),
                    ownerUserId);
            return @event;
        }

        public void AddSiegeEvent(GameAggregate game, CastleAggregate castle, CastleAggregate destinationCastle, List<SoldierAggregate> soldiers)
        {
            var battleAt = GetBattleTime(game.Speed);
            destinationCastle.Siege = new SiegeAggregate
            {
                Id = Guid.NewGuid(),
                OwnerUserId = castle.OwnerUserId,
                Soldiers = soldiers,
                BattleAt = battleAt,
                SiegeAt = DateTime.UtcNow
            };
            // create siege event
            AddSiegeEvent(game, destinationCastle, soldiers);
        }

        public void AddSiegeEvent(GameAggregate game, CastleAggregate destinationCastle, List<SoldierAggregate> soldiers)
        {
            if (destinationCastle.Siege == null)
                return;
            var battleAt = GetBattleTime(game.Speed);
            _domain.AddEvent(game.Id,
                new SiegeCastleEvent(destinationCastle.Siege.OwnerUserId,
                destinationCastle.Id,
                soldiers,
                DateTime.UtcNow,
                battleAt));
        }

        public DateTime GetBattleTime(GameSpeed speed)
        {
            var siegeTime = _gameSettings.SiegeTime * GameSpeedHelper.GetSpeedValue(speed);
            return DateTime.UtcNow.AddMinutes(siegeTime);
        }

        public RevenueCointEvent RevenueCoinEvent(GameSpeed speed)
        {
            return new RevenueCointEvent(DateTime.UtcNow, DateTime.UtcNow.Add(GetRevenueTimeBySpeed(speed)));
        }

        public UpkeepCoinEvent UpkeepCoinEvent(GameSpeed speed)
        {
            return new UpkeepCoinEvent(DateTime.UtcNow, DateTime.UtcNow.Add(GetUpkeepTimeBySpeed(speed)));
        }

        public TimeSpan GetUpkeepTimeBySpeed(GameSpeed speed)
        {
            return TimeSpan.FromMinutes(_gameSettings.UpkeepTime * GameSpeedHelper.GetSpeedValue(speed));
        }

        public double GetCoinsToUpgradeCastle(double strength)
        {
            switch ((int)strength)
            {
                case 2:
                    return 100;
                case 3:
                    return 200;
                default:
                    return 0;
            }
        }

        public void CreateSoldierIfNeed(GameAggregate snap)
        {
            CreateSoldierIfNeed(snap, Army.Blue);
            CreateSoldierIfNeed(snap, Army.Red);
        }

        public double CalculateCoin(GameAggregate game, CastleAggregate castle)
        {
            var revenueCoins = _gameSettings.RevenueCoins * GameSpeedHelper.GetSpeedValue(game.Speed);
            return revenueCoins;
        }

        public double CalculateUpkeepCoin(Guid id, CastleAggregate castle)
        {
            return 0;
            //double coins = 0;
            //var soldiers = castle.GetAvailableSoldiers() ?? new List<SoldierAggregate>();
            //foreach (var soldier in soldiers)
            //{
            //    coins += GetUpkeepCoinBySoldierType(castle, soldier.CastleTroopType.ResourceType);
            //}
            //return coins;
        }

        public double GetUpkeepCoinBySoldierType(GameAggregate game, Army army, string troopType)
        {
            var troopTypeData = (army == Army.Blue ? game.UserTroopTypes : game.OpponentTroopTypes)?.FirstOrDefault(e => e.ResourceType == troopType);
            if (troopTypeData != null)
            {
                return troopTypeData.UpkeepCoins;
            }
            return 0;
        }

        public TimeSpan GetRevenueTimeBySpeed(GameSpeed speed)
        {
            return TimeSpan.FromMinutes(_gameSettings.RevenueTime * GameSpeedHelper.GetSpeedValue(speed));
        }

        public bool CreateSoldierIfNeed(GameAggregate game, Army army)
        {
            var @event = GetCreateSoldierIfNeedCreate(game, army);
            if (@event != null)
            {
                _domain.AddEvent(game.Id, @event);
                return true;
            }
            return false;
        }
        public CreateSoldierEvent GetCreateSoldierIfNeedCreate(GameAggregate game, Army army)
        {
            if (!game.IsProductionState(army))
                return null;
            var productEvent = _domain.GetNotExecuteEvents<CreateSoldierEvent>(game.Id) ?? new List<CreateSoldierEvent>();
            var hasInProgressEvent = productEvent.Any(e => e.Army == army);
            if (hasInProgressEvent)
                return null;
            var troopType = game.GetDefaultTroopType(army);
            var needCoin = GetUpkeepCoinBySoldierType(game, army, troopType);
            bool canProduction;
            if (army == Army.Blue)
                canProduction = game.UserCoins > needCoin;
            else
                canProduction = game.OpponentCoins > needCoin;
            if (canProduction)
                return GetCreateSoldierEvent(game.Id, army,
                    troopType,
                    game.GetUserId(army));
            return null;
        }
    }
}
