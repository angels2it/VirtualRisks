using System;
using System.Collections.Generic;
using CastleGo.Domain.Bases;
using CastleGo.Entities;

namespace CastleGo.Domain.Events
{
    public class StartBattalionEvent : EventBase
    {
        public Guid MovementId { get; set; }
        public Guid CastleId { get; set; }
        public Guid DestinationCastleId { get; set; }

        public List<string> Soldiers { get; set; }
        public Route Route { get; set; }
        public StartBattalionEvent(Guid movementId, Guid castleId, Guid destinationCastleId, List<string> soldiers, Route route, string userId, DateTime runningAt, DateTime executeAt) : base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, executeAt)
        {
            MovementId = movementId;
            CastleId = castleId;
            DestinationCastleId = destinationCastleId;
            Soldiers = soldiers;
            Route = route;
        }
    }
}