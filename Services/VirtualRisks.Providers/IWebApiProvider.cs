// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Providers.IWebApiProvider
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace CastleGo.Providers
{
  /// <summary>Web api helper</summary>
  public interface IWebApiProvider
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="resource"></param>
    /// <param name="parameters"></param>
    /// <param name="token"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    Task<TOut> GetAsync<TOut>(string baseUrl, string resource, List<Parameter> parameters = null, string token = "");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    RestClient GetClient(string baseUrl, string token = "");
  }
}
