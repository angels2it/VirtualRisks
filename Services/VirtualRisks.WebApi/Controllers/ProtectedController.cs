// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Controllers.ProtectedController
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System.Web.Http;

namespace CastleGo.WebApi.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Authorize]
  public abstract class ProtectedController : ApiController
  {
  }
}
