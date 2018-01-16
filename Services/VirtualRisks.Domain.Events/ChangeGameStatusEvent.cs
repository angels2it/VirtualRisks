
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CastleGo.Entities;

namespace CastleGo.Domain.Events
{
    public class InitGameEvent : EventBase
    {
        public string UserId { get; set; }
        public string UserHeroId { get; set; }
        public string OpponentId { get; set; }
        public string OpponentHeroId { get; set; }
        public bool SelfPlaying { get; set; }
        public GameSpeed Speed { get; set; }
        public GameDifficulfy Difficulty { get; set; }
        public GameArmySetting UserArmySetting { get; set; }
        public List<CastleTroopType> UserTroopTypes { get; set; }
        public List<CastleTroopType> OpponentTroopTypes { get; set; }
        public List<string> UserProducedTroopTypes { get; set; }
        public List<string> OpponentProducedTroopTypes { get; set; }

        public InitGameEvent() : base(DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }

    public class UpdateOpponentInfoEvent : EventBase
    {
        public string OpponentId { get; set; }
        public string OpponentHeroId { get; set; }
        public UpdateOpponentInfoEvent() : base(DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }
    public class ChangeGameStatusEvent : EventBase
    {
        public GameStatus Status { get; set; }

        public ChangeGameStatusEvent()
          : base(DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }

    public class SelectOpponentArmySettingEvent : EventBase
    {
        public GameArmySetting ArmySetting { get; set; }
        public SelectOpponentArmySettingEvent() : base(DateTime.UtcNow, DateTime.UtcNow)
        {

        }
    }
}
