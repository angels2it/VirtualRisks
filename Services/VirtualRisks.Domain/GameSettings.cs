using System;

namespace CastleGo.Domain
{
    public class GameSettings
    {
        /// <summary>
        /// minute
        /// </summary>
        public int ProductionTime { get; set; }
        /// <summary>
        /// meter per minute
        /// </summary>
        public double BattalionMovementSpeed { get; set; }
        /// <summary>
        /// minute
        /// </summary>
        public int SiegeTime { get; set; }
        /// <summary>
        /// meter
        /// </summary>
        public int DistanceHeroARoundCastleThreshold { get; set; }

        public double HeroStayInCastleTime { get; set; }
        public double RevenueTime { get; set; }
        public double RevenueCoins { get; set; }
        public double UpkeepTime { get; set; }
        public double WallStrength { get; set; }

        private double GetBattalionMovementSpeed()
        {
            return BattalionMovementSpeed * 1.0 / 60; // m/s
        }
        public double GetMovementSpeedOfGame(double speedValue)
        {
            return GetBattalionMovementSpeed();
        }

        public double GetRevenueCoinOfGame(double speedValue)
        {
            throw new NotImplementedException();
        }
    }
}
