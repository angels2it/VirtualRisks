// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Interfaces.IMessageHandler`1
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using CastleGo.Domain.Bases;
using System.Threading.Tasks;

namespace CastleGo.Domain.Interfaces
{
  public interface IMessageHandler<in T> where T : MessageBase
  {
    Task Handle(T message);
  }
}
