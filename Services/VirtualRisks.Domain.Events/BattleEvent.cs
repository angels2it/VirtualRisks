using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class BattleEvent : EventBase
    {
        public Guid AtCastleId { get; set; }
        public BattleEvent(Guid id, DateTime runningAt, DateTime executeAt) : base(runningAt, executeAt)
        {
            AtCastleId = id;
        }
    }
}
