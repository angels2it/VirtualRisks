using System;

namespace CastleGo.Shared.Games.Events
{
    public class FailedAttackCastleEventModel : AttackEventModel
    {
        public Guid CastleId { get; set; }
    }
}
