// Decompiled with JetBrains decompiler
// Type: CastleGo.DataAccess.Models.UpdateGameStateDataModel
// Assembly: CastleGo.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C835FEBF-C9B9-4D1F-8860-3FDF1F4179C3
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.DataAccess.dll

using CastleGo.Shared.Common;

namespace CastleGo.DataAccess.Models
{
  public class UpdateGameStateDataModel
  {
    public GameStatus Status { get; set; }

    public int RedCastleAmount { get; set; }

    public int BlueCastleAmount { get; set; }

    public int NeutrualCastleAmount { get; set; }
  }
}
