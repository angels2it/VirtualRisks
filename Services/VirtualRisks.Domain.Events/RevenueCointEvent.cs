using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Events
{
    public class RevenueCointEvent : EventBase
    {
        public RevenueCointEvent(DateTime runningAt, DateTime executeAt) : base(Guid.NewGuid().ToString(), runningAt, executeAt)
        {
        }
    }
}
