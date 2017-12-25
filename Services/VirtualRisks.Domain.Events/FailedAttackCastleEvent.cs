using System;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class FailedAttackCastleEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public FailedAttackCastleEvent(string userId, Guid castleId, BattleLogAggregate log, DateTime runningAt) :
            base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, runningAt)
        {
            CastleId = castleId;
            BattleLog = log;
        }
    }
}