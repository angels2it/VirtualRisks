// Decompiled with JetBrains decompiler
// Type: CastleGo.Domain.Bases.MessageBase
// Assembly: CastleGo.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F80A2754-D8FD-4961-9EDC-4401D257BC1C
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Domain.dll

using System;
using TinyMessenger;

namespace CastleGo.Domain.Bases
{
  public class MessageBase : TinyMessageBase
  {
    public string Id { get; set; }

    public DateTime EndedAt { get; set; }

    public MessageBase(object sender)
      : base(sender)
    {
    }
  }
}
