using System;

namespace CastleGo.Shared.Games.Events
{
    public class SiegeCastleEventModel : EventBaseModel
    {
        public string SiegeBy { get; set; }
        public Guid CastleId { get; set; }
    }
}
