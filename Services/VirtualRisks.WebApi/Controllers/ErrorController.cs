// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Controllers.ErrorController
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using CastleGo.WebApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CastleGo.WebApi.Controllers
{
  /// <summary>
  /// Represents controller that exposes routes for error viewing.
  /// </summary>
  [ApiExplorerSettings(IgnoreApi = true)]
  public class ErrorController : ApiController
  {
    /// <summary>
    /// Intercepts rout not found error in order to show it as friendly message on the screen.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>404 with <see cref="T:CastleGo.WebApi.Models.ErrorInfoModel" /> object as content.</returns>
    [HttpGet]
    [HttpPost]
    [HttpPut]
    [HttpPatch]
    [HttpDelete]
    [HttpHead]
    [HttpOptions]
    public HttpResponseMessage NotFound(string path)
    {
      ErrorInfoModel errorInfoModel = new ErrorInfoModel();
      errorInfoModel.Message = "Route doesn't exist.";
      DateTimeOffset utcNow = (DateTimeOffset) DateTime.UtcNow;
      errorInfoModel.TimeStamp = utcNow;
      Uri requestUri = this.Request.RequestUri;
      errorInfoModel.RequestUri = requestUri;
      Guid correlationId = this.Request.GetCorrelationId();
      errorInfoModel.ErrorId = correlationId;
      return this.Request.CreateResponse<ErrorInfoModel>(HttpStatusCode.NotFound, errorInfoModel);
    }
  }
}
