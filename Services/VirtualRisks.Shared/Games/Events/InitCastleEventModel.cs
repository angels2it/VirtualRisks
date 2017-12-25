using CastleGo.Shared.Common;

namespace CastleGo.Shared.Games.Events
{
    public class InitCastleEventModel : EventBaseModel
    {
        public string Name { get; set; }
        public Army Army { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserId { get; set; }

        public PositionModel Position { get; set; }
    }
}
