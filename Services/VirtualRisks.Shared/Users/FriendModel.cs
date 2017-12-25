// Decompiled with JetBrains decompiler
// Type: CastleGo.Shared.Users.FriendModel
// Assembly: CastleGo.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 157A9D10-4624-400D-A7F3-8771FF84E829
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Application.dll

namespace CastleGo.Shared.Users
{
    public class FriendModel : BaseModel
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public PositionModel Position { get; set; }
    }
}
