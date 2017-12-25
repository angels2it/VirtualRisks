// Decompiled with JetBrains decompiler
// Type: CastleGo.Shared.PagingResult`1
// Assembly: CastleGo.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 157A9D10-4624-400D-A7F3-8771FF84E829
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Application.dll

using System.Collections.Generic;

namespace CastleGo.Shared
{
  public class PagingResult<T> where T : class
  {
    public List<T> Items { get; set; }

    public long Total { get; set; }

    public bool CanLoadMore { get; set; }

    public PagingResult()
    {
      this.Items = new List<T>();
    }
  }
}
