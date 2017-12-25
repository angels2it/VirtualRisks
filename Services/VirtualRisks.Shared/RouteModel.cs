using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared
{
    public class RouteModel
    {
        public List<RouteStepModel> Steps { get; set; }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
    }

    public class RouteStepModel
    {
        public PositionModel StartLocation { get; set; }
        public PositionModel EndLocation { get; set; }
        public double Distance { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
