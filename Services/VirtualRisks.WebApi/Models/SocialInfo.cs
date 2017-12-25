// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.SocialInfo
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Newtonsoft.Json;
#pragma warning disable 1591

namespace CastleGo.WebApi.Models
{
  public class SocialInfo
  {
    private string _fullName { get; set; }

    [JsonProperty("id")]
    public string SocialId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("middle_name")]
    public string MiddleName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("gender")]
    public string Gender { get; set; }

    [JsonProperty("birthday")]
    public string Birthday { get; set; }

    [JsonProperty("location")]
    public string Location { get; set; }

    [JsonProperty("picture")]
    public string Picture { get; set; }

    public string FullName
    {
      get
      {
        if (!string.IsNullOrEmpty(this._fullName))
          return this._fullName;
        if (string.IsNullOrEmpty(this.MiddleName))
          return string.Format("{0} {1}", (object) this.FirstName, (object) this.LastName);
        return string.Format("{0} {1} {2}", (object) this.FirstName, (object) this.MiddleName, (object) this.LastName);
      }
      set
      {
        this._fullName = value;
      }
    }
  }
}
