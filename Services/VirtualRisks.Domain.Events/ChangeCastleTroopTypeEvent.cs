using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Domain.Bases;
using CastleGo.Shared.Common;

namespace CastleGo.Domain.Events
{
    public class ChangeCastleTroopTypeEvent : EventBase
    {
        public Guid CastleId { get; set; }
        public string TroopType { get; set; }
        public ChangeCastleTroopTypeEvent(Guid castleId, string troopType) : base(Guid.NewGuid().ToString(), DateTime.UtcNow, DateTime.UtcNow)
        {
            CastleId = castleId;
            TroopType = troopType;
        }
    }
}
