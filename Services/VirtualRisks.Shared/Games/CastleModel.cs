﻿using CastleGo.Shared.Common;
using System.Collections.Generic;

namespace CastleGo.Shared.Games
{
    public class GenerateCastleData
    {
        public List<CastleModel> Castles { get; set; }
        public List<CastleRouteModel> Routes { get; set; }
    }
    public class CastleRouteModel
    {
        public CastleRouteModel(CastleModel fromCastle, CastleModel toCastle)
        {
            FromCastle = fromCastle;
            ToCastle = toCastle;
        }

        public CastleModel FromCastle { get; set; }
        public CastleModel ToCastle { get; set; }
    }
    public class CastleModel : BaseModel
    {
        public Army Army { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserId { get; set; }

        public PositionModel Position { get; set; }

        public int MaxResourceLimit { get; set; }

        public List<string> Soldiers { get; set; }

        public CastleTroopTypeModel CastleTroopType { get; set; }
        
        public string Name { get; set; }
        public List<string> ProducedTroopTypes { get; set; }
        public List<CastleTroopTypeModel> TroopTypes { get; set; }
        public double Strength { get; set; }
        public int Index { get; set; }
        public int RouteCount { get; set; }
        public bool IsAdded { get; set; }
    }
}
