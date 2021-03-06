using System.Collections.Generic;
using System.Linq;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Games.Events;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class GameStateUpdate
    {
        public List<CastleRouteDto> Routes { get; set; }
        public List<CastleStateModel> Castles { get; set; }
        public List<SoldierModel> UserSoldiers { get; set; }
        public List<SoldierModel> OpponentSoldiers { get; set; }
        public bool IsBlue { get; set; }
        public List<WebApi.RestClient.Models.EventBaseModel> Events { get; internal set; }

        public int GetSoldiersAmount()
        {
            if (IsBlue)
                return UserSoldiers?.Count ?? 0;
            return OpponentSoldiers?.Count ?? 0;
        }

        public List<SoldierModel> GetMySoldiers()
        {
            if (IsBlue)
                return UserSoldiers;
            return OpponentSoldiers;
        }

        internal Army GetMyArmy()
        {
            if (IsBlue)
                return Army.Blue;
            return Army.Red;
        }

        public List<string> GetDragableCastles(string id)
        {
            return Routes.Where(r => r.FromCastle == id || r.ToCastle == id)
                .SelectMany(r => new[] { r.FromCastle, r.ToCastle }).Distinct().Except(new[] { id }).ToList();
        }

        public CastleByDistanceModel GetNearestCastle(List<string> castles, double lat, double lng)
        {
            return Castles.Where(c => castles.Contains(c.Id)).Select(c => new CastleByDistanceModel
            {
                Castle = c,
                Distance = MapHelpers.GetDistance(c.Position.Lat, c.Position.Lng, lat,
                    lng)
            }).OrderBy(d => d.Distance).First();
        }

        public List<CastleStateModel> GetMyCastles()
        {
            var myArmy = GetMyArmy();
            return Castles.Where(e => e.Army == myArmy).ToList();
        }
        public List<string> GetMyCastlesId()
        {
            var myArmy = GetMyArmy();
            return Castles.Where(e => e.Army == myArmy).Select(e => e.Id).ToList();
        }
        public List<string> GetOpponentCastlesId()
        {
            var myArmy = GetMyArmy();
            return Castles.Where(e => e.Army != myArmy).Select(e => e.Id).ToList();
        }
    }

    public class CastleByDistanceModel
    {
        public CastleStateModel Castle { get; set; }
        public double Distance { get; set; }
    }
}