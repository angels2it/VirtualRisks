using System.Collections.Generic;
using Android.Gms.Maps.Model;
using Java.Lang;

namespace Rangstrup.Xam.Plugin.Maps.Droid
{

    public class SphericalUtil
    {
        private SphericalUtil()
        {
        }

        /**
         * Returns the heading from one LatLng to another LatLng. Headings are
         * expressed in degrees clockwise from North within the range [-180,180).
         * @return The heading in degrees clockwise from north.
         */

        public static double computeHeading(LatLng from, LatLng to)
        {
            // http://williams.best.vwh.net/avform.htm#Crs
            double fromLat = Math.ToRadians(from.Latitude);
            double fromLng = Math.ToRadians(from.Longitude);
            double toLat = Math.ToRadians(to.Latitude);
            double toLng = Math.ToRadians(to.Longitude);
            double dLng = toLng - fromLng;
            double heading = Math.Atan2((
                Math.Sin(dLng) * Math.Cos(toLat)),
                Math.Cos(fromLat) * Math.Sin(toLat) - Math.Sin(fromLat) * Math.Cos(toLat) * Math.Cos(dLng));
            return MathUtil.wrap(Math.ToDegrees(heading), -180, 180);
        }

        /**
         * Returns the LatLng resulting from moving a distance from an origin
         * in the specified heading (expressed in degrees clockwise from north).
         * @param from     The LatLng from which to start.
         * @param distance The distance to travel.
         * @param heading  The heading in degrees clockwise from north.
         */

        public static LatLng computeOffset(LatLng from, double distance, double heading)
        {
            distance /= MathUtil.EARTH_RADIUS;
            heading = Math.ToRadians(heading);
            // http://williams.best.vwh.net/avform.htm#LL
            double fromLat = Math.ToRadians(from.Latitude);
            double fromLng = Math.ToRadians(from.Longitude);
            double cosDistance = Math.Cos(distance);
            double sinDistance = Math.Sin(distance);
            double sinFromLat = Math.Sin(fromLat);
            double cosFromLat = Math.Cos(fromLat);
            double sinLat = cosDistance * sinFromLat + sinDistance * cosFromLat * Math.Cos(heading);
            double dLng = Math.Atan2((
                sinDistance * cosFromLat * Math.Sin(heading)),
                cosDistance - sinFromLat * sinLat);
            return new LatLng(Math.ToDegrees(Math.Sin(sinLat)), Math.ToDegrees(fromLng + dLng));
        }

        /**
         * Returns the location of origin when provided with a LatLng destination,
         * meters travelled and original heading. Headings are expressed in degrees
         * clockwise from North. This function returns null when no solution is
         * available.
         * @param to       The destination LatLng.
         * @param distance The distance travelled, in meters.
         * @param heading  The heading in degrees clockwise from north.
         */

        public static LatLng computeOffsetOrigin(LatLng to, double distance, double heading)
        {
            heading = Math.ToRadians(heading);
            distance /= MathUtil.EARTH_RADIUS;
            // http://lists.maptools.org/pipermail/proj/2008-October/003939.html
            double n1 = Math.Cos(distance);
            double n2 = Math.Sin(distance) * Math.Cos(heading);
            double n3 = Math.Sin(distance) * Math.Sin(heading);
            double n4 = Math.Sin(Math.ToRadians(to.Latitude));
            // There are two solutions for b. b = n2 * n4 +/- Math.Sqrt(), one solution results
            // in the latitude outside the [-90, 90] range. We first try one solution and
            // back off to the other if we are outside that range.
            double n12 = n1 * n1;
            double discriminant = n2 * n2 * n12 + n12 * n12 - n12 * n4 * n4;
            if (discriminant < 0)
            {
                // No real solution which would make sense in LatLng-space.
                return null;
            }
            double b = n2 * n4 + Math.Sqrt(discriminant);
            b /= n1 * n1 + n2 * n2;
            double a = (n4 - n2 * b) / n1;
            double fromLatRadians = Math.Atan2(a, b);
            if (fromLatRadians < -Math.Pi / 2 || fromLatRadians > Math.Pi / 2)
            {
                b = n2 * n4 - Math.Sqrt(discriminant);
                b /= n1 * n1 + n2 * n2;
                fromLatRadians = Math.Atan2(a, b);
            }
            if (fromLatRadians < -Math.Pi / 2 || fromLatRadians > Math.Pi / 2)
            {
                // No solution which would make sense in LatLng-space.
                return null;
            }
            double fromLngRadians = Math.ToRadians(to.Longitude) -
                                    Math.Atan2(n3, n1 * Math.Cos(fromLatRadians) - n2 * Math.Sin(fromLatRadians));
            return new LatLng(Math.ToDegrees(fromLatRadians), Math.ToDegrees(fromLngRadians));
        }

        /**
         * Returns the LatLng which lies the given fraction of the way between the
         * origin LatLng and the destination LatLng.
         * @param from     The LatLng from which to start.
         * @param to       The LatLng toward which to travel.
         * @param fraction A fraction of the distance to travel.
         * @return The interpolated LatLng.
         */

        public static LatLng interpolate(LatLng from, LatLng to, double fraction)
        {
            // http://en.wikipedia.org/wiki/Slerp
            double fromLat = Math.ToRadians(from.Latitude);
            double fromLng = Math.ToRadians(from.Longitude);
            double toLat = Math.ToRadians(to.Latitude);
            double toLng = Math.ToRadians(to.Longitude);
            double cosFromLat = Math.Cos(fromLat);
            double cosToLat = Math.Cos(toLat);

            // Computes Spherical interpolation coefficients.
            double angle = computeAngleBetween(from, to);
            double sinAngle = Math.Sin(angle);
            if (sinAngle < 1E-6)
            {
                return from;
            }
            double a = Math.Sin((1 - fraction) * angle) / sinAngle;
            double b = Math.Sin(fraction * angle) / sinAngle;

            // Converts from polar to vector and interpolate.
            double x = a * cosFromLat * Math.Cos(fromLng) + b * cosToLat * Math.Cos(toLng);
            double y = a * cosFromLat * Math.Sin(fromLng) + b * cosToLat * Math.Sin(toLng);
            double z = a * Math.Sin(fromLat) + b * Math.Sin(toLat);

            // Converts interpolated vector back to polar.
            double lat = Math.Atan2(z, Math.Sqrt(x * x + y * y));
            double lng = Math.Atan2(y, x);
            return new LatLng(Math.ToDegrees(lat), Math.ToDegrees(lng));
        }

        /**
         * Returns distance on the unit sphere; the arguments are in radians.
         */

        private static double distanceRadians(double lat1, double lng1, double lat2, double lng2)
        {
            return MathUtil.arcHav(MathUtil.havDistance(lat1, lat2, lng1 - lng2));
        }

        /**
         * Returns the angle between two LatLngs, in radians. This is the same as the distance
         * on the unit sphere.
         */

        public static double computeAngleBetween(LatLng from, LatLng to)
        {
            return distanceRadians(Math.ToRadians(from.Latitude), Math.ToRadians(from.Longitude),
                Math.ToRadians(to.Latitude), Math.ToRadians(to.Longitude));
        }

        /**
         * Returns the distance between two LatLngs, in meters.
         */

        public static double computeDistanceBetween(LatLng from, LatLng to)
        {
            return computeAngleBetween(from, to) * MathUtil.EARTH_RADIUS;
        }

        /**
         * Returns the length of the given path, in meters, on Earth.
         */

        public static double computeLength(List<LatLng> path)
        {
            if (path.Count < 2)
            {
                return 0;
            }
            double length = 0;
            LatLng prev = path[0];
            double prevLat = Math.ToRadians(prev.Latitude);
            double prevLng = Math.ToRadians(prev.Longitude);
            foreach (var point in path)
            {
                double lat = Math.ToRadians(point.Latitude);
                double lng = Math.ToRadians(point.Longitude);
                length += distanceRadians(prevLat, prevLng, lat, lng);
                prevLat = lat;
                prevLng = lng;
            }
            return length * MathUtil.EARTH_RADIUS;
        }

        /**
         * Returns the area of a closed path on Earth.
         * @param path A closed path.
         * @return The path's area in square meters.
         */

        public static double computeArea(List<LatLng> path)
        {
            return Math.Abs(computeSignedArea(path));
        }

        /**
         * Returns the signed area of a closed path on Earth. The sign of the area may be used to
         * determine the orientation of the path.
         * "inside" is the surface that does not contain the South Pole.
         * @param path A closed path.
         * @return The loop's area in square meters.
         */

        public static double computeSignedArea(List<LatLng> path)
        {
            return computeSignedArea(path, MathUtil.EARTH_RADIUS);
        }

        /**
         * Returns the signed area of a closed path on a sphere of given radius.
         * The computed area uses the same units as the radius squared.
         * Used by SphericalUtilTest.
         */

        static double computeSignedArea(List<LatLng> path, double radius)
        {
            int size = path.Count;
            if (size < 3)
            {
                return 0;
            }
            double total = 0;
            LatLng prev = path[(size - 1)];
            double prevTanLat = Math.Tan((Math.Pi / 2 - Math.ToRadians(prev.Latitude)) / 2);
            double prevLng = Math.ToRadians(prev.Longitude);
            // For each edge, accumulate the signed area of the triangle formed by the North Pole
            // and that edge ("polar triangle").
            foreach (var point in path)
            {
                double tanLat = Math.Tan((Math.Pi / 2 - Math.ToRadians(point.Latitude)) / 2);
                double lng = Math.ToRadians(point.Longitude);
                total += polarTriangleArea(tanLat, lng, prevTanLat, prevLng);
                prevTanLat = tanLat;
                prevLng = lng;
            }
            return total * (radius * radius);
        }

        /**
         * Returns the signed area of a triangle which has North Pole as a vertex.
         * Formula derived from "Area of a spherical triangle given two edges and the included angle"
         * as per "Spherical Trigonometry" by Todhunter, page 71, section 103, point 2.
         * See http://books.google.com/books?id=3uBHAAAAIAAJ&pg=PA71
         * The arguments named "tan" are tan((pi/2 - latitude)/2).
         */

        private static double polarTriangleArea(double tan1, double lng1, double tan2, double lng2)
        {
            double deltaLng = lng1 - lng2;
            double t = tan1 * tan2;
            return 2 * Math.Atan2(t * Math.Sin(deltaLng), 1 + t * Math.Cos(deltaLng));
        }
    }
}