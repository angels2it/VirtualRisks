using System;
using System.Collections.Generic;
using System.Text;

namespace CastleGo.Shared.Games.Events
{
    public class MoveSoldierEventModel : EventBaseModel
    {
        public Guid CastleId { get; set; }
        public List<string> Soldiers { get; set; }
    }
}
