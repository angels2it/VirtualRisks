﻿// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.InstagramData
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Newtonsoft.Json;
#pragma warning disable 1591

namespace CastleGo.WebApi.Models
{
  public class InstagramData
  {
    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("full_name")]
    public string FullName { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
  }
}
