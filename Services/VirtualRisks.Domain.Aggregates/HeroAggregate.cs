using System;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Aggregates
{
    public class HeroAggregate : Aggregate
    {
        public string UserId { get; set; }
        public Army Army { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
