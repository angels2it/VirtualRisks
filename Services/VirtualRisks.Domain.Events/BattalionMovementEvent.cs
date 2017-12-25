using System;
using System.Collections.Generic;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Entities;

namespace CastleGo.Domain.Events
{
    public class BattalionMovementEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public Guid DestinationCastleId { get; set; }
        public List<SoldierAggregate> Soldiers { get; set; }
        public Route Route { get; set; }
        public BattalionMovementEvent(Guid castleId, Guid destinationCastleId, List<SoldierAggregate> soldiers, Route route, string userId, DateTime runningAt, DateTime executeAt) : base(userId, runningAt, executeAt)
        {
            CastleId = castleId;
            DestinationCastleId = destinationCastleId;
            Soldiers = soldiers;
            Route = route;
        }
    }
}
