using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Games
{
    public class HeroAroundCastleModel
    {
        public string UserId { get; set; }
        public string HeroId { get; set; }
        public PositionModel Position { get; set; }
    }
}
