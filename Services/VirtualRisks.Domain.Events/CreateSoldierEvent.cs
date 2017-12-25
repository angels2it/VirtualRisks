using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;
using System;

namespace CastleGo.Domain.Events
{
    public class CreateSoldierEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public string TroopType { get; set; }

        public TimeSpan ProductionTime { get; set; }

        public CreateSoldierEvent(Guid id, string type, DateTime runningAt, TimeSpan produceTime, string userId)
          : base(string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId, runningAt, runningAt.Add(produceTime))
        {
            CastleId = id;
            ProductionTime = produceTime;
            TroopType = type;
        }
    }
}
