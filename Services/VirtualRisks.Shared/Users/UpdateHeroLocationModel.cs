using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Users
{
    public class UpdateHeroLocationModel
    {
        public PositionModel Position { get; set; }
        public string HeroId { get; set; }
    }
}
