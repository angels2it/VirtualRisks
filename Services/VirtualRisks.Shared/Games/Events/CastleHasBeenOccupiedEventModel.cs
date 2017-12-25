using System;

namespace CastleGo.Shared.Games.Events
{
    public class CastleHasBeenOccupiedEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public BattleLogModel BattleLog { get; set; }
    }
}
