using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class SiegeHasBeenOccupiedEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public SiegeHasBeenOccupiedEvent(string userId, Guid castleId, BattleLogAggregate battle, DateTime runningAt) : base(userId, runningAt, runningAt)
        {
            CastleId = castleId;
            BattleLog = battle;
        }
    }
}