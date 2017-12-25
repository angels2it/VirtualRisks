using System;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class HeroARoundCastleEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public string UserId { get; set; }
        public string HeroId { get; set; }

        public HeroARoundCastleEvent(Guid castleId, string userId, string heroId) : base(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
            UserId = userId;
            HeroId = heroId;
        }
    }
}
