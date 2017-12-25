// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Providers.MongoDbProvider
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using AspNet.Identity.MongoDB;
using CastleGo.WebApi.Models;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using MongoRepository;
using System.Configuration;

namespace CastleGo.WebApi.Providers
{
  internal class MongoDbProvider
  {
    internal static MongoDbSetting GetMongoDbSetting()
    {
      MongoClient mongoClient = new MongoClient(Util<string>.GetDefaultConnectionString());
      string appSetting = ConfigurationManager.AppSettings["ReadDBName"];
      if (string.IsNullOrEmpty(appSetting))
        throw new ConfigurationErrorsException("Please config db name!");
      IMongoDatabase database = mongoClient.GetDatabase(appSetting, (MongoDatabaseSettings) null);
      UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>((IUserStore<ApplicationUser>) new UserStore<ApplicationUser>(database.GetCollection<ApplicationUser>("User", (MongoCollectionSettings) null)));
      return new MongoDbSetting() { Client = (IMongoClient) mongoClient, Database = database, UserManager = userManager };
    }
  }
}
