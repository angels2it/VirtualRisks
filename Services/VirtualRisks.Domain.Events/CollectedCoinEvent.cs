using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Events
{
    public class CollectedCoinEvent : EventBase
    {
        public Army Army { get; set; }
        public double Coins { get; set; }
        public CollectedCoinEvent(double coins, Army army, string createdBy) : base(createdBy, DateTime.UtcNow, DateTime.UtcNow)
        {
            Army = army;
            Coins = coins;
        }
    }
}