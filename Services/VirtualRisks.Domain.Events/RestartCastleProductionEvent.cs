using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class RestartCastleProductionEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public RestartCastleProductionEvent(Guid castleId, string ownerId) : base(ownerId, DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
        }
    }
}