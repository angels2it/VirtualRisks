using System;
using System.Collections.Generic;
using CastleGo.Shared.Games;
using CastleGo.Shared.Users;

namespace CastleGo.Shared
{
    public class SiegeStateModel
    {
        public Guid Id { get; set; }
        public string OwnerUserId { get; set; }
        public UserModel OwnerUser { get; set; }
        public DateTime BattleAt { get; set; }
        public List<SoldierModel> Soldiers { get; set; }
        public DateTime SiegeAt { get; set; }
    }
}
