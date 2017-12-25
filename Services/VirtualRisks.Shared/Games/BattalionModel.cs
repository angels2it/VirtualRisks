using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Games
{
    public class BattalionModel
    {
        public string CastleId { get; set; }
        public string DestinationCastleId { get; set; }
        public List<string> Soldiers { get; set; }
        public Guid Id { get; set; }
        public int PercentOfSelectedSoldiers { get; set; }
        public bool MoveByPercent { get; set; }
    }

    public class BattalionModelResult
    {
        public Guid CastleId { get; set; }
        public Guid DestinationCastleId { get; set; }
        public DateTime RunningAt { get; set; }
        public RouteModel Route { get; set; }
        public string Id { get; set; }
    }
}
