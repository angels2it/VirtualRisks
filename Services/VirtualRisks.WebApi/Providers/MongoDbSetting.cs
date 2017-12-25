// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Providers.MongoDbSetting
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using CastleGo.WebApi.Models;
using MongoDB.Driver;

namespace CastleGo.WebApi.Providers
{
  internal class MongoDbSetting
  {
    public IMongoClient Client { get; set; }

    public IMongoDatabase Database { get; set; }

    public Microsoft.AspNet.Identity.UserManager<ApplicationUser> UserManager { get; set; }
  }
}
