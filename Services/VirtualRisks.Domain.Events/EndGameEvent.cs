using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Events
{
    public class EndGameEvent : EventBase
    {
        public Army Winner { get; set; }
        public EndGameEvent(Army winner, DateTime runningAt) : base(runningAt, runningAt)
        {
            Winner = winner;
        }
    }
}
