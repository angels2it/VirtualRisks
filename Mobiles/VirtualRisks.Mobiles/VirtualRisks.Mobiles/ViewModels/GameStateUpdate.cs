using System;
using System.Collections.Generic;
using System.Linq;
using VirtualRisks.WebApi.RestClient.Models;

namespace VirtualRisks.Mobiles.ViewModels
{
    public class GameStateUpdate
    {
        public List<CastleRouteDto> Routes { get; set; }
        public List<CastleStateModel> Castles { get; set; }
        public List<SoldierModel> UserSoldiers { get; set; }
        public List<SoldierModel> OpponentSoldiers { get; set; }
        public bool IsBlue { get; set; }
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

        internal string GetMyArmy()
        {
            if (IsBlue)
                return "Blue";
            return "Red";
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
                Distance = MapHelpers.GetDistance(c.Position.Lat.GetValueOrDefault(0), c.Position.Lng.GetValueOrDefault(0), lat,
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