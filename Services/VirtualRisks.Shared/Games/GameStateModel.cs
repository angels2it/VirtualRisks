using System;
using CastleGo.Shared.Common;
using System.Collections.Generic;
using CastleGo.Shared.Games.Events;
using Newtonsoft.Json.Linq;

namespace CastleGo.Shared.Games
{
    public class GameStateModel
    {
        public Guid Id { get; set; }
        public List<CastleStateModel> Castles { get; set; }

        public GameStatus Status { get; set; }

        public bool HasError { get; set; }
        public int StreamRevision { get; set; }
#if MOBILE
        public List<JObject> Events { get; set; }
#else
        public List<EventBaseModel> Events { get; set; }
#endif
        public List<BattalionMovementEventModel> BattalionMovements { get; set; }
        public string UserId { get; set; }
        public string UserHeroId { get; set; }
        public string OpponentId { get; set; }
        public string OpponentHeroId { get; set; }
        public bool SelfPlaying { get; set; }
        public GameSpeed Speed { get; set; }
        public GameDifficulfy Difficulty { get; set; }
        public double UserCoins { get; set; }
        public double OpponentCoins { get; set; }
        public List<string> UserProducedTroopTypes { get; internal set; }
        public List<SoldierModel> UserSoldiers { get; set; }
        public List<SoldierModel> OpponentSoldiers { get; set; }
    }

    public class CastleRouteStateModel
    {
        public string FromCastle { get; set; }
        public string ToCastle { get; set; }
        public RouteModel Route { get; set; }
    }
}
