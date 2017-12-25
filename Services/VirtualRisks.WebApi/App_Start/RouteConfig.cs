// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.RouteConfig
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Web.Http;

namespace CastleGo.WebApi
{
    /// <summary>Represents route configuration.</summary>
    public static class RouteConfig
    {
        /// <summary>Configures Web API routes.</summary>
        /// <param name="configuration"></param>
        public static HttpConfiguration UsingRoute(this HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute("Index", "", (object)new
            {
                controller = "Home",
                action = "Index"
            });
            configuration.Routes.MapHttpRoute("NotFound", "{*path}", (object)new
            {
                controller = "Error",
                action = "NotFound"
            });
            return configuration;
        }
    }
}
