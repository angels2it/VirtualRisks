using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Games.Events
{
    public class OccupiedSiegeInCastleEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public BattleLogModel BattleLog { get; set; }
    }
}
