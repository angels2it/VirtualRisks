using System;

namespace CastleGo.Shared.Games.Events
{
    public class BattleAttackingLogModel
    {
        public string OwnerUserId { get; set; }
        public Guid AttackingSoldierId { get; set; }
        public Guid DefendingSoldierId { get; set; }
        public double Hit { get; set; }
        public int Booststrength { get; set; }
        public double WallStrength { get; set; }
    }
}