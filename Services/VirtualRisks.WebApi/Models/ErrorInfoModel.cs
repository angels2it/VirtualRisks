// Decompiled with JetBrains decompiler
// Type: CastleGo.WebApi.Models.ErrorInfoModel
// Assembly: CastleGo.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7693054B-E1BD-4660-BFEE-B6E9FD9A5C45
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.WebApi.dll

using System;

namespace CastleGo.WebApi.Models
{
  /// <summary>Represents error information that can be shown to user.</summary>
  public class ErrorInfoModel
  {
    /// <summary>Gets or sets error message.</summary>
    public string Message { get; set; }

    /// <summary>Gets or sets error date and time.</summary>
    public DateTimeOffset TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets URI, Web API rout that has failed to complete.
    /// </summary>
    public Uri RequestUri { get; set; }

    /// <summary>
    /// <see cref="T:System.Guid" /> value that represents correlation identifier that can be used for tracking purposes.
    /// </summary>
    public Guid ErrorId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ExeptionMessage { get; set; }
  }
}
