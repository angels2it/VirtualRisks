// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.FacebookFriendData
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace CastleGo.Providers.Models
{
  public class FacebookFriendData
  {
    [DeserializeAs(Name = "data")]
    [JsonProperty("data")]
    public List<FacebookFriend> Data { get; set; }

    [DeserializeAs(Name = "summary")]
    [JsonProperty("summary")]
    public FacebookSumaryData Summary { get; set; }

    [DeserializeAs(Name = "paging")]
    [JsonProperty("paging")]
    public FacebookPagingData Paging { get; set; }
  }
}
