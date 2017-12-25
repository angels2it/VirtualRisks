using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Games
{
    public class CheckBattalionMovementEventModel
    {
        public Guid GameId { get; set; }
        public Guid EventId { get; set; }
        public int StreamVersion { get; set; }
    }
}
