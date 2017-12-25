// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.FacebookUserAvatarDataData
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Newtonsoft.Json;

namespace CastleGo.Providers.Models
{
  public class FacebookUserAvatarDataData
  {
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("is_silhouette")]
    public bool IsSilhouette { get; set; }
  }
}
