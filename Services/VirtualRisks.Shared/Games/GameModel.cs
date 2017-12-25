using CastleGo.Shared.Common;
using CastleGo.Shared.Users;
using System;
using System.Collections.Generic;
using CastleGo.Application.Settings.Dtos;

namespace CastleGo.Shared
{
    public class GameModel : BaseModel
    {
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string OpponentId { get; set; }

        public string UserHeroId { get; set; }

        public string OpponentHeroId { get; set; }

        public GameStatus Status { get; set; }

        public List<string> Castles { get; set; }

        public PositionModel Position { get; set; }

        public UserModel User { get; set; }

        public HeroModel UserHero { get; set; }

        public UserModel Opponent { get; set; }

        public HeroModel OpponentHero { get; set; }

        public int RedCastleAmount { get; set; }

        public int BlueCastleAmount { get; set; }

        public int NeutrualCastleAmount { get; set; }
        public OpponentExtInfoModel OpponentExtInfo { get; set; }
        public bool SelfPlaying { get; set; }
        public GameSpeed Speed { get; set; }
        public GameDifficulfy Difficulty { get; set; }
        public GameArmySettingModel UserArmySetting { get; set; }
        public GameArmySettingModel OpponentArmySetting { get; set; }
    }
    /// <summary>
    /// It used to save ext data to define opponent
    /// </summary>
    public class OpponentExtInfoModel
    {
        public string Provider { get; set; }

        public string Key { get; set; }
        public string KeyName { get; set; }
    }
}
