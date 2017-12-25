using System;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class DefendedSiegeEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public DefendedSiegeEvent(Guid castleId, BattleLogAggregate log, string createdBy) : base(createdBy, DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
            BattleLog = log;
        }
    }

    public class FailedAttackSiegeEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public FailedAttackSiegeEvent(string userId, Guid castleId, BattleLogAggregate log) : base(userId, DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
            BattleLog = log;
        }
    }
}
