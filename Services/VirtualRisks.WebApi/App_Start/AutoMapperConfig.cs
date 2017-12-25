// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.AutoMapperConfig
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using AutoMapper;
using System;
using System.Web.Http;

namespace CastleGo.WebApi
{
  /// <summary>Automapper config</summary>
  public static class AutoMapperConfig
  {
    /// <summary>Config method</summary>
    public static HttpConfiguration UsingAutoMapper(this HttpConfiguration configuration)
    {
      Mapper.Initialize(cfg => cfg.AddProfiles(AppDomain.CurrentDomain.GetAssemblies()));
      return configuration;
    }
  }
}
