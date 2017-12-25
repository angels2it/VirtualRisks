using System;
using System.Collections.Generic;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Aggregates
{
    public class SiegeAggregate : Aggregate
    {
        public string OwnerUserId { get; set; }
        public List<SoldierAggregate> Soldiers { get; set; }
        public DateTime BattleAt { get; set; }
        public DateTime SiegeAt { get; set; }
    }
}
