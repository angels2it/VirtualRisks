using CastleGo.Domain.Bases;
using System;

namespace CastleGo.Domain.Events
{
    public class CreateGameEvent : EventBase
    {
        public CreateGameEvent()
          : base(DateTime.UtcNow, DateTime.UtcNow)
        {
        }
    }
}
