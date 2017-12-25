using CastleGo.Domain.Interfaces;
using System;

namespace CastleGo.Domain.Bases
{
    public abstract class EventBase : IEventBase
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime RunningAt { get; set; }

        public DateTime ExecuteAt { get; set; }
        protected EventBase(DateTime runningAt, DateTime executeAt)
        {
            Id = Guid.NewGuid();
            RunningAt = runningAt;
            ExecuteAt = executeAt;
        }
        protected EventBase(string createdBy, DateTime runningAt, DateTime executeAt) : this(runningAt, executeAt)
        {
            CreatedBy = createdBy;
        }
    }
}
