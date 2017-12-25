// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Interfaces.IDomainEventHandler`1
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using CastleGo.Domain.Bases;

namespace CastleGo.Domain.Interfaces
{
  public interface IDomainEventHandler<T> where T : IEventBase
  {
    bool Handle(DomainEventHandlerData<T> @event);
  }
}
