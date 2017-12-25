using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;
using System.Collections.Generic;
using CastleGo.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

        public bool CanProduce(CastleAggregate castle, double needCoins)
        {
            if (castle.Army == Army.Neutrual)
                return true;
            var coins = castle.Army == Army.Blue ? UserCoins : OpponentCoins;
            if (coins >= needCoins)
                return true;
            return false;
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
