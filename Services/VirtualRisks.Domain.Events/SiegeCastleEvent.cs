using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class SiegeCastleEvent : EventBase
    {
        public string SiegeBy { get; set; }
        public Guid CastleId { get; set; }
        public List<SoldierAggregate> Soldiers { get; set; }
        public SiegeCastleEvent(string siegeBy, Guid castleId, List<SoldierAggregate> soldiers, DateTime runningAt, DateTime executeAt) : base(runningAt, executeAt)
        {
            SiegeBy = siegeBy;
            CastleId = castleId;
            Soldiers = soldiers;
        }
    }
}
