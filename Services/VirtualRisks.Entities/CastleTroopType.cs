﻿using CastleGo.Shared.Common;
using System;
using System.Collections.Generic;
using MongoRepository;

namespace CastleGo.Entities
{
    public class GameArmySetting : Entity
    {
        public string Name { get; set; }
        public List<GameCastleSetting> Castles { get; set; }
    }

    public class GameCastleSetting : Entity
    {
        public string Name { get; set; }
    }
    public class CastleTroopType : Entity
    {
        public string ResourceType { get; set; }
        public double AttackStrength { get; set; }
        public int MinAttackStrength { get; set; }
        public int MaxAttackStrength { get; set; }

        public double Health { get; set; }
        public int MinHealth { get; set; }
        public int MaxHealth { get; set; }

        public double MovementSpeed { get; set; }
        public int MinMovementSpeed { get; set; }
        public int MaxMovementSpeed { get; set; }

        public TimeSpan ProductionSpeed { get; set; }
        public int MinProductionSpeed { get; set; }
        public int MaxProductionSpeed { get; set; }

        public int UpkeepCoins { get; set; }
        public int MinUpkeepCoins { get; set; }
        public int MaxUpkeepCoins { get; set; }

        public bool IsFlight { get; set; }
        public bool IsOverComeWalls { get; set; }
        public string Icon { get; set; }
        public string BlueArmyIcon { get; set; }
        public string RedArmyIcon { get; set; }
    }
}
