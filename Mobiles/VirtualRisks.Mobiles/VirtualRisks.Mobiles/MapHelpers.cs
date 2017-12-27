using System;

namespace VirtualRisks.Mobiles
{
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public static class MapHelpers
    {
       
        // Given three colinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        static bool OnSegment(Position p, Position q, Position r)
        {
            if (q.Latitude <= Math.Max(p.Latitude, r.Latitude) && q.Latitude >= Math.Min(p.Latitude, r.Latitude) && q.Longitude <= Math.Max(p.Longitude, r.Longitude) && q.Longitude >= Math.Min(p.Longitude, r.Longitude))
                return true;

            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are colinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        static int Orientation(Position p, Position q, Position r)
        {
            // See http://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            double val = (q.Longitude - p.Longitude)*(r.Latitude - q.Latitude) - (q.Latitude - p.Latitude)*(r.Longitude - q.Longitude);

            if (val == 0) return 0; // colinear

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        // lat =x
        public static bool DoBoundingBoxesIntersect(Position[] a, Position[] b)
        {
            var p1 = a[0];
            var q1 = a[1];
            var p2 = b[0];
            var q2 = b[1];

            // Find the four orientations needed for general and
            // special cases
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

            // p1, q1 and p2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }

        public static bool DoIntersectWith(this Position[] a, Position[] b)
        {
            return DoBoundingBoxesIntersect(a, b);
        }

        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1); // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a = Math.Sin(dLat/2)*Math.Sin(dLat/2) + Math.Cos(deg2rad(lat1))*Math.Cos(deg2rad(lat2))*Math.Sin(dLon/2)*Math.Sin(dLon/2);
            var c = 2*Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R*c; // Distance in km
            return d;
        }

        private static double deg2rad(double deg)
        {
            return deg*(Math.PI/180);
        }
    }
}
