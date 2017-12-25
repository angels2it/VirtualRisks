using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class CastleRevenueCointEvent : EventBase
    {
        public CastleRevenueCointEvent(Guid id, double coins, string createdBy, DateTime runningAt, DateTime executeAt) : base(createdBy, runningAt, executeAt)
        {
            CastleId = id;
            Coins = coins;
        }

        public Guid CastleId { get; set; }
        public double Coins { get; set; }
    }
}