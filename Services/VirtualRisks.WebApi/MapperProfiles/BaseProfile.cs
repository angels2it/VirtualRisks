// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.MapperProfiles.BaseProfile
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using AutoMapper;
using CastleGo.Application;
using CastleGo.Shared;
using MongoRepository;

namespace CastleGo.WebApi.MapperProfiles
{
  /// <summary>
  /// 
  /// </summary>
  public class BaseProfile : Profile
  {
    /// <summary>
    /// 
    /// </summary>
    public BaseProfile()
    {
      this.CreateMap<Entity, BaseModel>();
      this.CreateMap<BaseModel, Entity>();
      this.CreateMap<BaseModel, BaseDto>();
    }
  }
}
