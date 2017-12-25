using System;

namespace CastleGo.Shared.Games.Events
{
    public class DefendedSiegeEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public BattleLogModel BattleLog { get; set; }
    }
}
