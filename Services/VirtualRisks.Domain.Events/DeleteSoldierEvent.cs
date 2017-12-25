// Decompiled with JetBrains decompiler
using CastleGo.Domain.Bases;
using System;

namespace CastleGo.Domain.Events
{
    public class DeleteSoldierEvent : EventBase
    {
        public DeleteSoldierEvent(Guid soldierId)
          : base(DateTime.UtcNow, DateTime.UtcNow)
        {
            SoldierId = soldierId;
        }

        public Guid SoldierId { get; set; }
    }
}
