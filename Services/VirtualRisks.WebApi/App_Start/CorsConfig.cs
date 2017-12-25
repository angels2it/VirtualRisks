// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.CorsConfig
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using Microsoft.Owin;
using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;

namespace CastleGo.WebApi
{
  /// <summary>Represents CORS configuration.</summary>
  public class CorsConfig
  {
    /// <summary>
    /// Instance of <see cref="T:Microsoft.Owin.Cors.CorsOptions" /> that is set to allow all by default.
    /// </summary>
    public static CorsOptions Options = CorsOptions.AllowAll;

    /// <summary>
    /// Initializes and configures <see cref="T:Microsoft.Owin.Cors.CorsOptions" /> instance.
    /// </summary>
    /// <param name="origins">String of allowed origins delimited by: ';'</param>
    public static void ConfigureCors(string origins)
    {
      if (string.IsNullOrWhiteSpace(origins))
        return;
      CorsPolicy corsPolicy = new CorsPolicy()
      {
        AllowAnyMethod = true,
        AllowAnyHeader = true
      };
      corsPolicy.Origins.ToList<string>().AddRange((IEnumerable<string>) origins.Split(';'));
      if (!corsPolicy.Origins.Any<string>())
        return;
      CorsConfig.Options = new CorsOptions()
      {
        PolicyProvider = (ICorsPolicyProvider) new CorsPolicyProvider()
        {
          PolicyResolver = (Func<IOwinRequest, Task<CorsPolicy>>) (context => Task.FromResult<CorsPolicy>(corsPolicy))
        }
      };
    }
  }
}
