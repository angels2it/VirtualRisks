using System;

namespace Rangstrup.Xam.Plugin.Maps.Droid
{
    public static class MathUtil
    {
        /**
         * The earth's radius, in meters.
         * Mean radius as defined by IUGG.
         */
        public static double EARTH_RADIUS = 6371009;

        /**
         * Restrict x to the range [low, high].
         */
        public static double clamp(double x, double low, double high)
        {
            return x < low ? low : (x > high ? high : x);
        }

        /**
         * Wraps the given value into the inclusive-exclusive interval between min and max.
         * @param n   The value to wrap.
         * @param min The minimum.
         * @param max The maximum.
         */
        public static double wrap(double n, double min, double max)
        {
            return (n >= min && n < max) ? n : (mod(n - min, max - min) + min);
        }

        /**
         * Returns the non-negative remainder of x / m.
         * @param x The operand.
         * @param m The modulus.
         */
        public static double mod(double x, double m)
        {
            return ((x % m) + m) % m;
        }

        /**
         * Returns mercator Y corresponding to latitude.
         * See http://en.wikipedia.org/wiki/Mercator_projection .
         */
        public static double mercator(double lat)
        {
            return Math.Log(Math.Tan(lat * 0.5 + Math.PI / 4));
        }

        /**
         * Returns latitude from mercator Y.
         */
        public static double inverseMercator(double y)
        {
            return 2 * Math.Atan((Math.Exp(y)) - Math.PI / 2);
        }

        /**
         * Returns haversine(angle-in-radians).
         * hav(x) == (1 - cos(x)) / 2 == sin(x / 2)^2.
         */
        public static double hav(double x)
        {
            double sinHalf = Math.Sin(x * 0.5);
            return sinHalf * sinHalf;
        }

        /**
         * Computes inverse haversine. Has good numerical stability around 0.
         * arcHav(x) == acos(1 - 2 * x) == 2 * asin(sqrt(x)).
         * The argument must be in [0, 1], and the result is positive.
         */
        public static double arcHav(double x)
        {
            return 2 * Math.Asin(Math.Sqrt(x));
        }

        // Given h==hav(x), returns sin(abs(x)).
        public static double sinFromHav(double h)
        {
            return 2 * Math.Sqrt(h * (1 - h));
        }

        // Returns hav(asin(x)).
        public static double havFromSin(double x)
        {
            double x2 = x * x;
            return x2 / (1 + Math.Sqrt(1 - x2)) * .5;
        }

        // Returns sin(arcHav(x) + arcHav(y)).
        public static double sinSumFromHav(double x, double y)
        {
            double a = Math.Sqrt(x * (1 - x));
            double b = Math.Sqrt(y * (1 - y));
            return 2 * (a + b - 2 * (a * y + b * x));
        }

        /**
         * Returns hav() of distance from (lat1, lng1) to (lat2, lng2) on the unit sphere.
         */
        public static double havDistance(double lat1, double lat2, double dLng)
        {
            return hav(lat1 - lat2) + hav(dLng) * Math.Cos(lat1) * Math.Cos(lat2);
        }
    }
}