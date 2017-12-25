using System;

namespace CastleGo.Shared
{
    public class PositionModel
    {
        public PositionModel(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public PositionModel()
        {
        }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public string Address { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
