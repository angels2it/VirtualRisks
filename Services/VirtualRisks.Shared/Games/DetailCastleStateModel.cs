using System;
using CastleGo.Shared.Games.Events;
using CastleGo.Shared.Users;
using System.Collections.Generic;
using CastleGo.Shared.Common;
#if MOBILE
using VirtualRisks.Mobiles.Models;
#endif

namespace CastleGo.Shared.Games
{
    public class DetailCastleStateModel : CastleStateModel
    {
        public HeroModel Owner { get; set; }

        public UserModel OwnerUser { get; set; }
#if MOBILE
        public List<MobileSoldierModel> Soldiers { get; set; }
#else
        public List<SoldierModel> Soldiers { get; set; }
#endif
        public int StreamRevision { get; set; }
        public List<EventBaseModel> Events { get; set; }
        public bool CanProductionSoldier { get; set; }
        public List<HeroStateModel> Heroes { get; set; }
        public double Revenue { get; set; }
        public TimeSpan RevenueTime { get; set; }
        public TimeSpan UpkeepTime { get; set; }
        public bool IsNotEnoughCoinForProduction { get; set; }
        public string CurrentTroopType { get; set; }
        public List<CastleTroopTypeModel> AvailableTroopTypes { get; set; }
        public bool CanChangeTroopType { get; set; }
        public string ProduceTroopType { get; set; }
        public ProductionState ProductionState { get; set; }
        public double Strength { get; set; }
        public Army CurrentUserArmy { get; set; }
        public bool CanUpgrade { get; set; }
    }

    public class HeroStateModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Army Army { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
