using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class UpkeepCoinEvent : EventBase
    {
        public UpkeepCoinEvent(DateTime runningAt, DateTime executeAt) : base(runningAt, executeAt)
        {
        }
    }
}