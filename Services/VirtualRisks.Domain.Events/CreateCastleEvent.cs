// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Events.CreateCastleEvent
// Assembly: CastleGo.Domain.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C6CD0066-BB88-4FCD-8A7D-5FBC45AA5350
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.Events.dll

using CastleGo.Domain.Bases;
using System;

namespace CastleGo.Domain.Events
{
  public class CreateCastleEvent : EventBase
  {
    public CreateCastleEvent()
      : base(DateTime.UtcNow, DateTime.UtcNow)
        {
    }
  }
}
