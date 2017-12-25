using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class HeroLeftCastleEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public string UserId { get; set; }
        public string HeroId { get; set; }
        public HeroLeftCastleEvent(Guid castleId, string userId, string heroId) : base(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
            UserId = userId;
            HeroId = heroId;
        }
    }
}
