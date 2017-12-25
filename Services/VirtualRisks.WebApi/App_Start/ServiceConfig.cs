// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.ServiceConfig
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace CastleGo.WebApi
{
  /// <summary>
  /// Represents configuration for <see cref="T:System.Web.Http.ExceptionHandling.IExceptionHandler" /> and <see cref="T:System.Web.Http.ExceptionHandling.IExceptionLogger" />.
  /// </summary>
  public static class ServiceConfig
  {
    /// <summary>
    /// COnfigures custom implementations for: <see cref="T:System.Web.Http.ExceptionHandling.IExceptionHandler" /> and <see cref="T:System.Web.Http.ExceptionHandling.IExceptionLogger" />.
    /// </summary>
    /// <param name="configuration"></param>
    public static HttpConfiguration UsingService(this HttpConfiguration configuration)
    {
      configuration.Services.Replace(typeof (IExceptionHandler), (object) new ApiExceptionHandler());
      configuration.Services.Add(typeof (IExceptionLogger), (object) new ApiExceptionLogger());
      return configuration;
    }
  }
}
