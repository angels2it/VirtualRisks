// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.HandlerRegistrar
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using CastleGo.Domain.Bases;
using CastleGo.Domain.Interfaces;
using System;

namespace CastleGo.Domain
{
  public class HandlerRegistrar : IHandlerRegistrar
  {
    public void RegisterHandler<T>(Func<DomainEventHandlerData<T>, bool> handler) where T : IEventBase
    {
      EventHandler.Add<T>(handler);
    }

    public void RegisterMessageHandler<T>(Action<T> handler) where T : MessageBase
    {
      MessageHandler.Add<T>(handler);
    }
  }
}
