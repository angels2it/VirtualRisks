using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class MoveSoldierEvent:EventBase
    {
        public Guid CastleId { get; set; }
        public List<string> Soldiers { get; set; }
        public MoveSoldierEvent(Guid id, List<string> soldiers, string createdBy, DateTime runningAt, DateTime executeAt) : base(createdBy, runningAt, executeAt)
        {
            CastleId = id;
            Soldiers = soldiers;
        }
    }
}
