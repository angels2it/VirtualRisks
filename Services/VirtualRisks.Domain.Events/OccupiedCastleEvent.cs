using System;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class OccupiedCastleEvent : EventBase
    {
        public string OwnerUserId { get; set; }
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public OccupiedCastleEvent(string userId, Guid castleId, BattleLogAggregate log, DateTime runningAt) 
            : base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, runningAt)
        {
            OwnerUserId = userId;
            CastleId = castleId;
            BattleLog = log;
        }
    }

    public class CastleHasBeenOccupiedEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public CastleHasBeenOccupiedEvent(string userId, Guid castleId, BattleLogAggregate log, DateTime runningAt) : base(userId, runningAt, runningAt)
        {
            CastleId = castleId;
            BattleLog = log;
        }
    }
}
