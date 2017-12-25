using System;
using System.Collections.Generic;

namespace CastleGo.Entities
{
    public class Route
    {
        public List<RouteStep> Steps { get; set; }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
    }

    public class RouteStep
    {
        public Position StartLocation { get; set; }
        public Position EndLocation { get; set; }
        public int Distance { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
