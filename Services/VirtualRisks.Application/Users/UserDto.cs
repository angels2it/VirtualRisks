// Decompiled with JetBrains decompiler
// Type: CastleGo.Application.Users.UserDto
// Assembly: CastleGo.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 157A9D10-4624-400D-A7F3-8771FF84E829
// Assembly location: C:\Users\Evil\Desktop\CastleGo-master\CastleGo-master\Services\CastleGo.WebApi\bin\CastleGo.Application.dll

using CastleGo.Shared.Users;
using System.Collections.Generic;
using CastleGo.Shared.Common;

namespace CastleGo.Application.Users
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public List<LoginsDto> Logins { get; set; }

        public bool IsAdmin { get; set; }

        public string Avatar { get; set; }

        public List<HeroModel> Heroes { get; set; }
        public List<NotifyTokenDto> Tokens { get; set; }
    }

    public class NotifyTokenDto
    {
        public MobileDevice Device { get; set; }
        public string Token { get; set; }
    }
}
