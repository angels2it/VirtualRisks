using System;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class DefendedCastleEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public DefendedCastleEvent(string userId, Guid castleId, BattleLogAggregate log, DateTime runningAt) : base(userId, runningAt, runningAt)
        {
            CastleId = castleId;
            BattleLog = log;
        }
    }
}
