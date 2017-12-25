using CastleGo.Domain.Bases;
using CastleGo.Entities;
using CastleGo.Shared.Common;
using System;
using System.Collections.Generic;

namespace CastleGo.Domain.Events
{
    public class InitCastleEvent : EventBase
    {
        public string Name { get; set; }
        public Army Army { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserId { get; set; }

        public Position Position { get; set; }

        public int MaxResourceLimit { get; set; }

        public List<Guid> Soldiers { get; set; }
        public List<string> ProducedTroopTypes { get; set; }
        public List<CastleTroopType> TroopTypes { get; set; }
        public double Strength { get; set; }

        public InitCastleEvent()
          : base(DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }
}
