// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Providers.IFacebookProvider
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using CastleGo.Providers.Models;

namespace CastleGo.Providers
{
  /// <summary>Facebook api provider</summary>
  public interface IFacebookProvider
  {
    /// <summary>Get friends</summary>
    /// <param name="token"></param>
    /// <param name="nextPageToken"></param>
    /// <returns></returns>
    Task<List<FacebookFriend>> Friends(string token, string nextPageToken = "");

    /// <summary>Get user info by token</summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<FacebookUserData> UserInfo(string token);
  }
}
