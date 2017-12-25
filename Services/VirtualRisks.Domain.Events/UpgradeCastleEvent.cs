using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class UpgradeCastleEvent: EventBase
    {
        public Guid CastleId { get; set; }
        public UpgradeCastleEvent(Guid castleId, string createdBy) : base(createdBy, DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
        }
    }
}
