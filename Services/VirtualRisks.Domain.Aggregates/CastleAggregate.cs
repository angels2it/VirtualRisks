using CastleGo.Domain.Bases;
using CastleGo.Entities;
using CastleGo.Shared.Common;
using System.Collections.Generic;
using System.Linq;

namespace CastleGo.Domain.Aggregates
{
    public class CastleAggregate : Aggregate
    {
        public string Name { get; set; }
        public int SoldiersAmount { get; set; }

        public List<SoldierAggregate> Soldiers { get; set; }

        public Army Army { get; set; }

        public Position Position { get; set; }

        public int MaxResourceLimit { get; set; }

        public string OwnerUserId { get; set; }

        public string OwnerId { get; set; }
        public SiegeAggregate Siege { get; set; }
        public List<HeroAggregate> Heroes { get; set; }
        public double Strength { get; set; }
        public ProductionState ProductionState { get; set; }

        public List<SoldierAggregate> GetAvailableSoldiers()
        {
            return Soldiers?.Where(e => !e.IsDead && e.CastleTroopType?.Health > 0).ToList() ?? new List<SoldierAggregate>();
        }

        public void UpdateSoldierAmount()
        {
            SoldiersAmount = Soldiers?.Count(e => !e.IsDead) ?? 0;
        }
        public void SuspendProduction()
        {
            ProductionState = ProductionState.Suspended;
        }

        public void RestartProduction()
        {
            ProductionState = ProductionState.OnGoing;
        }

        public bool IsProductionState()
        {
            return ProductionState == ProductionState.OnGoing;
        }
    }
}
