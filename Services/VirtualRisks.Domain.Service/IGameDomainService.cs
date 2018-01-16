using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Entities;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Service
{
    public interface IGameDomainService
    {
        bool CreateSoldierIfNeed(GameAggregate game, Army army);
        CreateSoldierEvent GetCreateSoldierIfNeedCreate(GameAggregate game, Army army);
        BattleLogAggregate AttackSequence(Guid id, CastleAggregate atCastle, string attackingId, string defendingId, List<SoldierAggregate> attacking, List<SoldierAggregate> defending, int attackingBooststrength = 0, int defendingBooststrength = 0);

        CreateSoldierEvent GetCreateSoldierEvent(Guid gameId, Army army, string troopType, string ownerUserId, bool immediately = false);
        void AddSiegeEvent(GameAggregate game, CastleAggregate castle, CastleAggregate destinationCastle, List<SoldierAggregate> soldiers);
        void AddSiegeEvent(GameAggregate snap, CastleAggregate destinationCastle, List<SoldierAggregate> soldiers);
        DateTime GetBattleTime(GameSpeed speed);
        RevenueCointEvent RevenueCoinEvent(GameSpeed speed);
        UpkeepCoinEvent UpkeepCoinEvent(GameSpeed speed);
        double CalculateCoin(GameAggregate game, CastleAggregate castle);
        double CalculateUpkeepCoin(Guid id, CastleAggregate castle);
        double GetUpkeepCoinBySoldierType(GameAggregate game, Army army, string troopType);
        TimeSpan GetRevenueTimeBySpeed(GameSpeed speed);
        TimeSpan GetUpkeepTimeBySpeed(GameSpeed speed);
        double GetCoinsToUpgradeCastle(double strength);
        void CreateSoldierIfNeed(GameAggregate snap);
    }
}
