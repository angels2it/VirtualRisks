using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class CastleUpkeepCointEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public double Coins { get; set; }
        public CastleUpkeepCointEvent(Guid castleId, double coins, string createdBy, DateTime runningAt, DateTime executeAt) : base(createdBy, runningAt, executeAt)
        {
            CastleId = castleId;
            Coins = coins;
        }
    }
}