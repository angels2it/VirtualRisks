using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;
using System;

namespace CastleGo.Domain.Events
{
    public class CreateSoldierEvent : EventBase
    {
        public string TroopType { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public Army Army { get; set; }

        public CreateSoldierEvent(Army army, string type, DateTime runningAt, TimeSpan produceTime, string userId)
          : base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, runningAt.Add(produceTime))
        {
            Army = army;
            ProductionTime = produceTime;
            TroopType = type;
        }
    }
}
