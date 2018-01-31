
using AutoMapper;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Events;
using CastleGo.Entities;
using CastleGo.Shared;
using CastleGo.Shared.Games;
using CastleGo.Shared.Games.Events;

namespace CastleGo.Application.AutoMapperProfiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<Game, GameModel>();
            CreateMap<CastleRoute, CastleRouteDto>();
            CreateMap<GameModel, Game>();
            CreateMap<Castle, CastleModel>();
            CreateMap<CastleModel, Castle>();
            CreateMap<CastleModel, InitCastleEvent>()
                .ForMember(e => e.Id, opt => opt.Ignore());
            CreateMap<CastleTroopTypeModel, CastleTroopType>();
            CreateMap<CastleTroopType, CastleTroopTypeModel>();
            CreateMap<CastleAggregate, CastleStateModel>();
            CreateMap<CastleAggregate, DetailCastleStateModel>();
            CreateMap<SoldierAggregate, SoldierModel>();
            CreateMap<SiegeAggregate, SiegeStateModel>();
            CreateMap<HeroAggregate, HeroStateModel>()
                .ForMember(e => e.Id, opt => opt.MapFrom(s => s.Id.ToString()));

            CreateMap<EventBase, EventBaseModel>()
                .Include<InitGameEvent, InitGameEventModel>()
                .Include<CreateGameEvent, CreateGameEventModel>()
                .Include<ChangeGameStatusEvent, ChangeGameStatusEventModel>()
                .Include<CreateCastleEvent, CreateCastleEventModel>()
                .Include<InitCastleEvent, InitCastleEventModel>()
                .Include<CreateSoldierEvent, CreateSoldierEventModel>()
                .Include<BattalionMovementEvent, BattalionMovementEventModel>()
                .Include<BattleEvent, BattleEventModel>()
                .Include<SiegeCastleEvent, SiegeCastleEventModel>()
                .Include<StartBattalionEvent, StartBattalionEventModel>()
                .Include<OccupiedCastleEvent, OccupiedCastleEventModel>()
                .Include<DefendedCastleEvent, DefendedCastleEventModel>()
                .Include<BattleVersusSiegeEvent, BattleVersusSiegeEventModel>()
                .Include<DefendedSiegeEvent, DefendedSiegeEventModel>()
                .Include<OccupiedSiegeInCastleEvent, OccupiedSiegeInCastleEventModel>()
                .Include<FailedAttackCastleEvent, FailedAttackCastleEventModel>()
                .Include<FailedAttackSiegeEvent, FailedAttackSiegeEventModel>()
                .Include<SiegeHasBeenOccupiedEvent, SiegeHasBeenOccupiedEventModel>()
                .Include<CastleHasBeenOccupiedEvent, CastleHasBeenOccupiedEventModel>()
                 .Include<EndGameEvent, EndGameEventModel>()
                 .Include<MoveSoldierEvent, MoveSoldierEventModel>();


            CreateMap<InitGameEvent, InitGameEventModel>();
            CreateMap<CreateGameEvent, CreateGameEventModel>();
            CreateMap<ChangeGameStatusEvent, ChangeGameStatusEventModel>();
            CreateMap<CreateCastleEvent, CreateCastleEventModel>();
            CreateMap<InitCastleEvent, InitCastleEventModel>();
            CreateMap<CreateSoldierEvent, CreateSoldierEventModel>();
            CreateMap<BattalionMovementEvent, BattalionMovementEventModel>();
            CreateMap<BattleEvent, BattleEventModel>();
            CreateMap<SiegeCastleEvent, SiegeCastleEventModel>();
            CreateMap<StartBattalionEvent, StartBattalionEventModel>();

            CreateMap<OccupiedCastleEvent, OccupiedCastleEventModel>();
            CreateMap<DefendedCastleEvent, DefendedCastleEventModel>();

            CreateMap<BattleVersusSiegeEvent, BattleVersusSiegeEventModel>();
            CreateMap<DefendedSiegeEvent, DefendedSiegeEventModel>();
            CreateMap<BattleVersusSiegeEvent, BattleVersusSiegeEventModel>();
            CreateMap<DefendedSiegeEvent, DefendedSiegeEventModel>();
            CreateMap<DefendedSiegeEvent, DefendedSiegeEventModel>();
            CreateMap<FailedAttackCastleEvent, FailedAttackCastleEventModel>();
            CreateMap<CastleHasBeenOccupiedEvent, CastleHasBeenOccupiedEventModel>();

            CreateMap<OccupiedSiegeInCastleEvent, OccupiedSiegeInCastleEventModel>();
            CreateMap<SiegeHasBeenOccupiedEvent, SiegeHasBeenOccupiedEventModel>();
            CreateMap<FailedAttackSiegeEvent, FailedAttackSiegeEventModel>();
            CreateMap<EndGameEvent, EndGameEventModel>();
            CreateMap<MoveSoldierEvent, MoveSoldierEventModel>();

            CreateMap<RouteModel, Route>();
            CreateMap<Route, RouteModel>();
            CreateMap<RouteStepModel, RouteStep>();
            CreateMap<RouteStep, RouteStepModel>();



            CreateMap<Aggregate, BaseModel>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<BattleLogAggregate, BattleLogModel>();
            CreateMap<BattleAttackingLogAggregate, BattleAttackingLogModel>();
        }
    }
}
