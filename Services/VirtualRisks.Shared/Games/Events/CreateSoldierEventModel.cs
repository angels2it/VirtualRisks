using System;
using CastleGo.Shared.Common;

namespace CastleGo.Shared.Games.Events
{
    public class CreateSoldierEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public string TroopType { get; set; }

        public TimeSpan ProductionTime { get; set; }
    }
}
