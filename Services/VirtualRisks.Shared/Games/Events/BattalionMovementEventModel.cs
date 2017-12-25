using System;
using System.Collections.Generic;

namespace CastleGo.Shared.Games.Events
{
    public class BattalionMovementEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public Guid DestinationCastleId { get; set; }
        public List<string> Soldiers { get; set; }
        public RouteModel Route { get; set; }
    }
}
