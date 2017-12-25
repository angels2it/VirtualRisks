using CastleGo.Shared.Common;
using System;

namespace CastleGo.Shared.Games
{
    public class CastleStateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int SoldiersAmount { get; set; }

        public CastleTroopTypeModel CastleTroopType { get; set; }

        public DateTime ProduceExecuteAt { get; set; }

        public PositionModel Position { get; set; }

        public Army Army { get; set; }
        public string OwnerUserId { get; set; }

        public string OwnerId { get; set; }
        public SiegeStateModel Siege { get; set; }
    }
}
