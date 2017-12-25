// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.ApplicationUser
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CastleGo.WebApi.Models
{
  /// <summary>
  /// 
  /// </summary>
  [BsonIgnoreExtraElements]
  public class ApplicationUser : IdentityUser, IUser<string>
  {
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime Updated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public CreatorRef CreatedBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public CreatorRef UpdatedBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime DeactiveDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      ClaimsIdentity claimsIdentity = await manager.CreateIdentityAsync(this, "ApplicationCookie");
      ClaimsIdentity userIdentity = claimsIdentity;
      claimsIdentity = (ClaimsIdentity) null;
      return userIdentity;
    }
  }
}
