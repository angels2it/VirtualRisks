
using CastleGo.Domain.Bases;
using CastleGo.Entities;

namespace CastleGo.Domain.Aggregates
{
    public class SoldierAggregate : Aggregate
    {
        public CastleTroopType CastleTroopType { get; set; }

        public bool IsDead { get; set; }
    }
}
