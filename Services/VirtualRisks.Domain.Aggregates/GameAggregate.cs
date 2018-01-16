using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;
using System.Collections.Generic;
using CastleGo.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace CastleGo.Domain.Aggregates
{
    public class GameAggregate : Aggregate
    {
        public GameStatus Status { get; set; }
        public string UserId { get; set; }
        public string UserHeroId { get; set; }
        public string OpponentId { get; set; }
        public string OpponentHeroId { get; set; }
        public List<CastleAggregate> Castles { get; set; }
        public List<BattleLogAggregate> Battles { get; set; }
        public Army Winner { get; set; }
        public bool SelfPlaying { get; set; }
        public GameSpeed Speed { get; set; }
        public GameDifficulfy Difficulty { get; set; }
        public double UserCoins { get; set; }
        public double OpponentCoins { get; set; }
        public GameArmySetting UserArmySetting { get; set; }
        public GameArmySetting OpponentArmySetting { get; set; }
        public List<CastleTroopType> UserTroopTypes { get; set; }
        public List<SoldierAggregate> UserSoldiers { get; set; }
        public List<string> UserProducedTroopTypes { get; set; }
        public List<SoldierAggregate> OpponentSoldiers { get; set; }
        public List<string> OpponentProducedTroopTypes { get; set; }
        public List<CastleTroopType> OpponentTroopTypes { get; set; }
        public int UserSoldiersAmount { get; set; }

        public bool CanProduce(CastleAggregate castle, double needCoins)
        {
            if (castle.Army == Army.Neutrual)
                return true;
            var coins = castle.Army == Army.Blue ? UserCoins : OpponentCoins;
            if (coins >= needCoins)
                return true;
            return false;
        }
        public CastleTroopType GetTroopTypeData(Army army, string troopType)
        {
            if (army == Army.Blue)
                return UserTroopTypes.FirstOrDefault(e => e.ResourceType == troopType);
            return OpponentTroopTypes.FirstOrDefault(e => e.ResourceType == troopType);
        }
        public string GetDefaultTroopType(Army army)
        {
            if (army == Army.Blue)
            {
                if (UserProducedTroopTypes == null || UserProducedTroopTypes.Count == 0)
                    return string.Empty;
                return UserProducedTroopTypes.First();
            }
            if (OpponentProducedTroopTypes == null || OpponentProducedTroopTypes.Count == 0)
                return string.Empty;
            return OpponentProducedTroopTypes.First();
        }

        public bool IsProductionState(Army army)
        {
            if (army == Army.Blue)
                return UserProductionState == ProductionState.OnGoing;
            return OpponentProductionState == ProductionState.OnGoing;
        }

        public ProductionState OpponentProductionState { get; set; }

        public ProductionState UserProductionState { get; set; }
        public int OpponentSoldierAmount { get; set; }

        public string GetUserId(Army army)
        {
            if (army == Army.Blue)
                return UserId;
            return OpponentId;
        }
    }

    public class BattleLogAggregate : Aggregate
    {
        public List<BattleAttackingLogAggregate> Logs { get; set; }
        public List<SoldierAggregate> Attacking { get; set; }
        public List<SoldierAggregate> Defending { get; set; }
    }

    public class BattleAttackingLogAggregate
    {
        public string OwnerUserId { get; set; }
        public Guid AttackingSoldierId { get; set; }
        public Guid DefendingSoldierId { get; set; }
        public double Hit { get; set; }
        public int Booststrength { get; set; }
        public double WallStrength { get; set; }
    }
}
