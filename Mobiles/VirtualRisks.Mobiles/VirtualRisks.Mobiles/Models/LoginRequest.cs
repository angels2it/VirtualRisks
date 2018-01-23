using CastleGo.Shared.Common;
using CastleGo.Shared.Games;

namespace VirtualRisks.Mobiles.Models
{
    public class LoginRequest
    {
        
    }
    public class MobileSoldierModel : SoldierModel
    {
        public bool IsSelected { get; set; }
        public Army Army { get; set; }
    }
}