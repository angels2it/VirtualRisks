// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Interfaces.IHandlerRegistrar
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using CastleGo.Domain.Bases;
using System;

namespace CastleGo.Domain.Interfaces
{
  public interface IHandlerRegistrar
  {
    void RegisterHandler<T>(Func<DomainEventHandlerData<T>, bool> handler) where T : IEventBase;
  }
}
