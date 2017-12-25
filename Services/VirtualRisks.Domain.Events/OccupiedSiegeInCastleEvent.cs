using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class OccupiedSiegeInCastleEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public List<SoldierAggregate> Soldiers { get; set; }
        public BattleLogAggregate BattleLog { get; set; }
        public OccupiedSiegeInCastleEvent(string userId, Guid castleId, BattleLogAggregate battle, List<SoldierAggregate> soldiers, DateTime runningAt) : 
            base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, runningAt)
        {
            CastleId = castleId;
            Soldiers = soldiers;
            BattleLog = battle;
        }
    }
}
