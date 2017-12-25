// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.FacebookPagingCursorData
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Newtonsoft.Json;

namespace CastleGo.Providers.Models
{
  public class FacebookPagingCursorData
  {
    [JsonProperty("before")]
    public string Before { get; set; }

    [JsonProperty("after")]
    public string After { get; set; }
  }
}
