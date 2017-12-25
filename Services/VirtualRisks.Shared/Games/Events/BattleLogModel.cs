using System.Collections.Generic;
using CastleGo.Shared.Common;

namespace CastleGo.Shared.Games.Events
{
    public class BattleLogModel : BaseModel
    {
        public List<BattleAttackingLogModel> Logs { get; set; }
        public List<SoldierModel> Attacking { get; set; }
        public List<SoldierModel> Defending { get; set; }
        public bool IsUserAttacking { get; set; }
        public Army AttackingArmy { get; set; }
        public Army DefendingArmy { get; set; }
    }
}