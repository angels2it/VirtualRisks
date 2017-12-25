using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class GotCoinPrizeEvent : EventBase
    {
        public GotCoinPrizeEvent(string createdBy) : base(createdBy, DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }
}
