using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class SuspendCastleProductionEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public SuspendCastleProductionEvent(Guid castleId, string ownerId) : base(ownerId, DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
        }
    }
}