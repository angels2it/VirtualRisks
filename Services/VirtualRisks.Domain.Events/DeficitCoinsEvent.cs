using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Events
{
    public class DeficitCoinsEvent : EventBase
    {
        public Army Army { get; set; }
        public double Coins { get; set; }
        public DeficitCoinsEvent(double coins, Army army, string createdBy) : base(createdBy, DateTime.UtcNow, DateTime.UtcNow)
        {
            Coins = coins;
            Army = army;
        }
    }
}
