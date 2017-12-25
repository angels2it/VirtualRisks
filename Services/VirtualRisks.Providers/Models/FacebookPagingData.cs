// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.FacebookPagingData
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace CastleGo.Providers.Models
{
  public class FacebookPagingData
  {
    [DeserializeAs(Name = "cursors")]
    [JsonProperty("cursors")]
    public FacebookPagingCursorData Cursors { get; set; }

    [DeserializeAs(Name = "next")]
    [JsonProperty("next")]
    public string Next { get; set; }
  }
}
