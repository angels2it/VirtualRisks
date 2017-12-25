using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class BattleVersusSiegeEvent : EventBase
    {
        public string AttackingUserId { get; set; }
        public Guid CastleId { get; set; }
        public List<SoldierAggregate> Battalion { get; set; }

        public BattleVersusSiegeEvent(string attackingUserId, Guid castleId, List<SoldierAggregate> battalion, DateTime runningAt) : base(runningAt, runningAt)
        {
            AttackingUserId = attackingUserId;
            CastleId = castleId;
            Battalion = battalion;
        }
    }
}
