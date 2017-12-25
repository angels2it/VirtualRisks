using System;

namespace CastleGo.Shared
{
    public static class Helpers
    {
        public static string GetBattalionDistanceText(double distance)
        {
            if (distance < 1000)
                return $"{distance} meters";
            var km = distance * 1.0 / 1000;
            return $"{Math.Round(km, 1)} kilometers";
        }
        public static double DistanceBetween(double lat1, double long1, double lat2, double long2)
        {
            double d1 = lat1 * (Math.PI / 180.0);
            double num1 = long1 * (Math.PI / 180.0);
            double d2 = lat2 * (Math.PI / 180.0);
            double num2 = long2 * (Math.PI / 180.0) - num1;
            return 6376.5 * (2.0 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0)))) * 1000.0;
        }
        public static string TimeToNowFormat(DateTime time)
        {
            if (time.Equals(default(DateTime)))
                return string.Empty;
            var span = DateTime.UtcNow.Subtract(time);
            int t;
            string tText;
            if (span.TotalDays > 1)
            {
                t = (int)span.TotalDays;
                tText = t == 1 ? "day" : "days";
            }
            else if (span.TotalHours > 1)
            {
                t = (int)span.TotalHours;
                tText = t == 1 ? "hour" : "hours";
            }
            else if (span.TotalMinutes > 1)
            {
                t = (int)span.TotalMinutes;
                tText = t == 1 ? "minute" : "minutes";
            }
            else
            {
                t = (int)span.TotalSeconds;
                tText = t == 1 ? "second" : "seconds";
            }
            return $"{t} {tText} ago";
        }
        public static string GetBattalionMovementTime(TimeSpan duration)
        {
            string text = "";
            if (duration.Minutes == 1)
                text += "1 minute, ";
            else
                text += $"{duration.Minutes} minutes, ";
            if (duration.Seconds == 1)
                text += "1 second";
            else
                text += $"{duration.Seconds} seconds";
            return text;
        }
    }
}
