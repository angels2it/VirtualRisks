// Decompiled with JetBrains decompiler
// Type: CastleGo.DataAccess.Models.PagingResult`1
// Assembly: CastleGo.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C835FEBF-C9B9-4D1F-8860-3FDF1F4179C3
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.DataAccess.dll

using System.Collections.Generic;

namespace CastleGo.DataAccess.Models
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
