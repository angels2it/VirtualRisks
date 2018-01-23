using System;
using System.Collections.Generic;
using System.Linq;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games.Events;
using BattalionModel = VirtualRisks.WebApi.RestClient.Models.BattalionModel;
using CastleRouteDto = CastleGo.Shared.CastleRouteDto;
using GameArmySettingModel = CastleGo.Application.Settings.Dtos.GameArmySettingModel;
using GameCastleSettingModel = CastleGo.Application.Settings.Dtos.GameCastleSettingModel;
using HeroModel = CastleGo.Shared.Users.HeroModel;
using OpponentExtInfoModel = CastleGo.Shared.OpponentExtInfoModel;
using PositionModel = CastleGo.Shared.PositionModel;
using RouteModel = CastleGo.Shared.RouteModel;
using RouteStepModel = CastleGo.Shared.RouteStepModel;
using SiegeStateModel = CastleGo.Shared.SiegeStateModel;
using SoldierModel = CastleGo.Shared.Games.SoldierModel;
using UserModel = CastleGo.Shared.Users.UserModel;

namespace VirtualRisks.Mobiles.Helpers
{
    public static class RestHelpers
    {
        public static CastleGo.Shared.Games.CastleTroopTypeModel ParseCastleTroopType(
            VirtualRisks.WebApi.RestClient.Models.CastleTroopTypeModel m)
        {
            if (m == null)
                return null;
            return new CastleGo.Shared.Games.CastleTroopTypeModel()
            {
                Id = m.Id,
                AttackStrength = m.AttackStrength.GetValueOrDefault(0),
                BlueArmyIcon = m.BlueArmyIcon,
                Health = m.Health.GetValueOrDefault(0),
                Icon = m.Icon,
                IsFlight = m.IsFlight.GetValueOrDefault(false),
                IsOverComeWalls = m.IsOverComeWalls.GetValueOrDefault(false),
                MaxAttackStrength = m.MaxAttackStrength.GetValueOrDefault(0),
                MaxHealth = m.MaxHealth.GetValueOrDefault(0),
                MaxMovementSpeed = m.MaxMovementSpeed.GetValueOrDefault(0),
                MaxProductionSpeed = m.MaxProductionSpeed.GetValueOrDefault(0),
                MaxUpkeepCoins = m.MaxUpkeepCoins.GetValueOrDefault(0),
                MinAttackStrength = m.MinAttackStrength.GetValueOrDefault(0),
                MinHealth = m.MinHealth.GetValueOrDefault(0),
                MinMovementSpeed = m.MinMovementSpeed.GetValueOrDefault(0),
                MinProductionSpeed = m.MinProductionSpeed.GetValueOrDefault(0),
                MinUpkeepCoins = m.MinUpkeepCoins.GetValueOrDefault(0),
                MovementSpeed = m.MovementSpeed.GetValueOrDefault(0),
                ProductionSpeed = TimeSpan.Parse(m.ProductionSpeed),
                RedArmyIcon = m.RedArmyIcon,
                ResourceType = m.ResourceType,
                UpkeepCoins = m.UpkeepCoins.GetValueOrDefault(0)
            };
        }

        public static CastleGo.Shared.Games.CastleStateModel ParseCastleState(WebApi.RestClient.Models.CastleStateModel e)
        {
            if (e == null)
                return null;
            return new CastleGo.Shared.Games.CastleStateModel()
            {
                Army = ParseArmy(e.Army),
                Id = e.Id,
                Position = ParsePosition(e.Position),
                CastleTroopType = ParseCastleTroopType(e.CastleTroopType),
                Name = e.Name,
                OwnerId = e.OwnerId,
                OwnerUserId = e.OwnerUserId,
                ProduceExecuteAt = e.ProduceExecuteAt.GetValueOrDefault(default(DateTime)),
                Siege = ParseSiege(e.Siege),
                SoldiersAmount = e.SoldiersAmount.GetValueOrDefault(0)
            };
        }

        public static SiegeStateModel ParseSiege(WebApi.RestClient.Models.SiegeStateModel siege)
        {
            if (siege == null)
                return null;
            return new SiegeStateModel()
            {
                Id = siege.Id.GetValueOrDefault(new Guid()),
                Soldiers = siege.Soldiers?.Select(ParseSoldier).ToList() ?? new List<SoldierModel>(),
                OwnerUserId = siege.OwnerUserId,
                BattleAt = siege.BattleAt.GetValueOrDefault(default(DateTime)),
                OwnerUser = ParseUser(siege.OwnerUser),
                SiegeAt = siege.SiegeAt.GetValueOrDefault(default(DateTime))
            };
        }

        private static UserModel ParseUser(WebApi.RestClient.Models.UserModel user)
        {
            if (user == null)
                return null;
            return new UserModel()
            {
                Name = user.Name,
                Avatar = user.Avatar
            };
        }

        public static SoldierModel ParseSoldier(WebApi.RestClient.Models.SoldierModel soldier)
        {
            if (soldier == null)
                return null;
            return new SoldierModel()
            {
                Id = soldier.Id,
                CastleTroopType = ParseCastleTroopType(soldier.CastleTroopType),
                UpkeepCoins = soldier.UpkeepCoins.GetValueOrDefault(0),
            };
        }

        public static Army ParseArmy(string army)
        {
            return (Army)Enum.Parse(typeof(Army), army);
        }

        public static PositionModel ParsePosition(WebApi.RestClient.Models.PositionModel position)
        {
            if (position == null)
                return null;
            return new CastleGo.Shared.PositionModel(position.Lat.GetValueOrDefault(0), position.Lng.GetValueOrDefault(0));
        }

        internal static BattalionModel CreateBattalion(string fromCastle, string toCastle, Guid? battalionId)
        {
            return new WebApi.RestClient.Models.BattalionModel
            {
                CastleId = fromCastle,
                DestinationCastleId = toCastle,
                MoveByPercent = true,
                PercentOfSelectedSoldiers = 100,
                Id = battalionId,
                Soldiers = new List<string>(),
                DateTime = DateTime.UtcNow
            };
        }

        public static CastleGo.Shared.GameModel ParseGame(WebApi.RestClient.Models.GameModel m)
        {
            if (m == null)
                return null;
            return new CastleGo.Shared.GameModel()
            {
                Id = m.Id,
                Position = ParsePosition(m.Position),
                CreatedBy = m.CreatedBy,
                Castles = m.Castles?.ToList() ?? new List<string>(),
                OpponentId = m.OpponentId,
                BlueCastleAmount = m.BlueCastleAmount.GetValueOrDefault(0),
                CreatedAt = m.CreatedAt.GetValueOrDefault(default(DateTime)),
                Difficulty = ParseGameDifficutly(m.Difficulty),
                NeutrualCastleAmount = m.NeutrualCastleAmount.GetValueOrDefault(0),
                Opponent = ParseUser(m.Opponent),
                OpponentArmySetting = ParseArmySetting(m.OpponentArmySetting),
                OpponentExtInfo = ParseExtInfo(m.OpponentExtInfo),
                OpponentHero = ParseHero(m.OpponentHero),
                OpponentHeroId = m.OpponentHeroId,
                RedCastleAmount = m.RedCastleAmount.GetValueOrDefault(0),
                Routes = m.Routes?.Select(ParseCastleRoute).ToList() ?? new List<CastleRouteDto>(),
                SelfPlaying = m.SelfPlaying.GetValueOrDefault(false),
                Speed = ParseSpeed(m.Speed),
                Status = ParseStatus(m.Status),
                User = ParseUser(m.User),
                UserArmySetting = ParseArmySetting(m.UserArmySetting),
                UserHero = ParseHero(m.UserHero),
                UserHeroId = m.UserHeroId
            };
        }

        private static GameStatus ParseStatus(string status)
        {
            return (GameStatus)Enum.Parse(typeof(GameStatus), status);
        }

        private static GameSpeed ParseSpeed(string speed)
        {
            return (GameSpeed)Enum.Parse(typeof(GameSpeed), speed);
        }

        private static CastleRouteDto ParseCastleRoute(WebApi.RestClient.Models.CastleRouteDto m)
        {
            if (m == null)
                return null;
            return new CastleRouteDto()
            {
                Route = ParseRoute(m.Route),
                FormattedRoute = m.FormattedRoute?.Select(ParsePosition).ToList() ?? new List<PositionModel>(),
                ToCastle = m.ToCastle,
                FromCastle = m.FromCastle
            };
        }

        private static RouteModel ParseRoute(WebApi.RestClient.Models.RouteModel m)
        {
            if (m == null)
                return null;
            return new RouteModel()
            {
                Duration = ParseTimeSpan(m.Duration),
                Distance = m.Distance.GetValueOrDefault(0),
                Steps = m.Steps?.Select(ParseStep).ToList() ?? new List<RouteStepModel>(),
            };
        }

        private static RouteStepModel ParseStep(WebApi.RestClient.Models.RouteStepModel m)
        {
            if (m == null)
                return null;
            return new RouteStepModel()
            {
                Duration = ParseTimeSpan(m.Duration),
                Distance = m.Distance.GetValueOrDefault(0),
                StartLocation = ParsePosition(m.StartLocation),
                EndLocation = ParsePosition(m.EndLocation)
            };
        }

        private static TimeSpan ParseTimeSpan(string m)
        {
            return TimeSpan.Parse(m);
        }

        private static HeroModel ParseHero(WebApi.RestClient.Models.HeroModel m)
        {
            if (m == null) return null;
            return new HeroModel()
            {
                Id = m.Id,
                Name = m.Name,
                Position = ParsePosition(m.Position)
            };
        }

        private static OpponentExtInfoModel ParseExtInfo(WebApi.RestClient.Models.OpponentExtInfoModel m)
        {
            if (m == null)
                return null;
            return new OpponentExtInfoModel()
            {
                Key = m.Key,
                KeyName = m.KeyName,
                Provider = m.Provider
            };
        }

        private static GameArmySettingModel ParseArmySetting(WebApi.RestClient.Models.GameArmySettingModel m)
        {
            if (m == null)
                return null;
            return new GameArmySettingModel()
            {
                Id = m.Id,
                Castles = m.Castles?.Select(ParseCatleSetting).ToList() ?? new List<GameCastleSettingModel>(),
                Name = m.Name
            };
        }

        public static GameCastleSettingModel ParseCatleSetting(WebApi.RestClient.Models.GameCastleSettingModel m)
        {
            if (m == null)
                return null;
            return new GameCastleSettingModel
            {
                Id = m.Id,
                Name = m.Name
            };
        }

        public static GameDifficulfy ParseGameDifficutly(string difficulty)
        {
            return (GameDifficulfy)Enum.Parse(typeof(GameDifficulfy), difficulty);
        }

        internal static EventBaseModel ParseEvent(WebApi.RestClient.Models.EventBaseModel m)
        {
            if (m == null)
                return null;
            return new EventBaseModel()
            {
                Id = m.Id.GetValueOrDefault(new Guid()),
                ExecuteAt = m.ExecuteAt.GetValueOrDefault(default(DateTime)),
                RunningAt = m.RunningAt.GetValueOrDefault(default(DateTime)),
                Type = m.Type
            };
        }
    }
}
