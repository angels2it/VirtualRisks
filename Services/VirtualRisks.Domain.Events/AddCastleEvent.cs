using CastleGo.Domain.Bases;
using System;

namespace CastleGo.Domain.Events
{
    public class AddCastleEvent : EventBase
    {
        public AddCastleEvent(string userId)
          : base(userId, DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }
}
