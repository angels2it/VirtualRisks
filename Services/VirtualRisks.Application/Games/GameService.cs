using AutoMapper;
using CastleGo.DataAccess;
using CastleGo.Domain.Aggregates;
using CastleGo.Domain.Events;
using CastleGo.Entities;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Users;
using MongoRepository;
using NEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CastleGo.Application.Games.Dtos;
using CastleGo.Application.Settings.Dtos;
using CastleGo.DataAccess.Models;
using CastleGo.Domain;
using CastleGo.Domain.Bases;
using CastleGo.Domain.Interfaces;
using CastleGo.Domain.Service;
using CastleGo.Shared.Games.Events;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CastleGo.Application.Games
{
    public class GameService : BaseService<GameModel, Game>, IGameService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IDomainService _domain;
        private readonly IStoreEvents _store;
        private readonly GameSettings _gameSettings;
        private readonly IGameDomainService _gameDomainService;
        private readonly IRepository<Game> _gameRepository;

        public GameService(IStoreEvents store, IRepository<Game> repository, IRepository<User> userRepository, IDomainService domain, GameSettings gameSettings, IGameDomainService gameDomainService, IRepository<Game> gameRepository)
          : base(repository)
        {
            _store = store;
            _userRepository = userRepository;
            _domain = domain;
            _gameSettings = gameSettings;
            _gameDomainService = gameDomainService;
            _gameRepository = gameRepository;
        }

        public async Task<GameStateModel> Build(Guid id, string userId, int streamVersion)
        {
            GameStateModel result = new GameStateModel()
            {
                Id = id
            };
            _domain.Build(id, InitGameSnapshot);
            result.HasError = false;
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            if (gameSnapshot == null)
            {
                result.HasError = true;
                return result;
            }
            result = GetGameStateBySnapshot(gameSnapshot);
            result.StreamRevision = latestSnapshot.StreamRevision;
            if (streamVersion >= 0)
                result.Events = GetUsedEvents(id, userId, streamVersion);
            result.BattalionMovements =
                Mapper.Map<List<BattalionMovementEventModel>>(_domain.GetNotExecuteEvents<BattalionMovementEvent>(id));
            await UpdateGameStateForReadData(id.ToString(), result);
            return result;
        }

        private List<EventBaseModel> GetUsedEvents(Guid id, string userId, int streamVersion)
        {
            var events = _domain.GetEvents(id, userId, streamVersion);
            return Mapper.Map<List<EventBaseModel>>(events.Where(e => EventConfig.PlayableEvents.Contains(e.GetType())).ToList());
        }

        private Task UpdateGameStateForReadData(string id, GameStateModel state)
        {
            IRepository<Game> repository = Repository;
            UpdateGameStateDataModel gameState = new UpdateGameStateDataModel
            {
                Status = state.Status,
                RedCastleAmount =
                    state.Castles?.Count(e => e.Army == Army.Red) ??
                    0,
                BlueCastleAmount =
                    state.Castles?.Count(e => e.Army == Army.Blue) ??
                    0,
                NeutrualCastleAmount =
                    state.Castles?.Count(e => e.Army == Army.Neutrual) ??
                    0
            };

            return repository.UpdateStateAsync(id, gameState);
        }

        private GameAggregate InitGameSnapshot(Guid id)
        {
            GameAggregate gameAggregate = new GameAggregate
            {
                Id = id,
                Castles = new List<CastleAggregate>(),
                Status = GameStatus.Requesting
            };
            return gameAggregate;
        }

        public async Task<string> CreateAsync(string userId, CreateGameModel model)
        {
            string userHeroId = string.Empty;
            string opponentHeroId = string.Empty;
            Task userTask = _userRepository.GetByIdAsync(userId).ContinueWith(r =>
            {
                User result = r.Result;
                if (result?.Heroes != null && result.Heroes.Count > 0)
                    userHeroId = result.Heroes[0].Id;
            });
            Task opponentTask = string.IsNullOrEmpty(model.OpponentId) ? Task.FromResult(true) : _userRepository.GetByIdAsync(model.OpponentId).ContinueWith(r =>
            {
                User result = r.Result;
                if (result?.Heroes != null && result.Heroes.Count > 0)
                    opponentHeroId = result.Heroes[0].Id;
            });
            await Task.WhenAll(userTask, opponentTask);
            if (string.IsNullOrEmpty(userHeroId) || (!string.IsNullOrEmpty(model.OpponentId) && string.IsNullOrEmpty(opponentHeroId)))
                throw new KeyNotFoundException("Hero not found");
            var gameId = Guid.NewGuid();
            GameModel gameModel = new GameModel
            {
                Id = gameId.ToString(),
                CreatedBy = userId,
                UserHeroId = userHeroId,
                OpponentId = model.OpponentId,
                OpponentHeroId = opponentHeroId,
                Status = GameStatus.Requesting,
                CreatedAt = DateTime.UtcNow,
                Position = new PositionModel
                {
                    Lat = model.Lat,
                    Lng = model.Lng
                },
                OpponentExtInfo = model.OpponentExtInfo,
                SelfPlaying = model.SelfPlaying,
                Speed = model.Speed,
                Difficulty = model.Difficulty,
                UserArmySetting = model.UserArmySetting
            };
            GameModel game = gameModel;
            using (IEventStream stream = _store.CreateStream(new Guid(game.Id)))
            {
                stream.Add(new EventMessage
                {
                    Body = new InitGameEvent
                    {
                        UserId = userId,
                        UserHeroId = userHeroId,
                        OpponentId = model.OpponentId,
                        OpponentHeroId = opponentHeroId,
                        SelfPlaying = model.SelfPlaying,
                        Speed = model.Speed,
                        Difficulty = model.Difficulty,
                        UserArmySetting = Mapper.Map<GameArmySetting>(model.UserArmySetting)
                    }
                });
                stream.Add(new EventMessage
                {
                    Body = new ChangeGameStatusEvent
                    {
                        Status = GameStatus.Requesting
                    }
                });

                stream.CommitChanges(Guid.NewGuid());
            }
            await Repository.AddAsync(Mapper.Map<Game>(game));
            _domain.Build(gameId, InitGameSnapshot);
            return game.Id;
        }

        public async Task AcceptedAsync(string id, string userId, string heroId = "")
        {
            var game = await Repository.GetByIdAsync(id);
            if (game == null)
                throw new KeyNotFoundException("Game.NotFound");
            Task userTask = !string.IsNullOrEmpty(heroId) ? Task.FromResult(true) : _userRepository.GetByIdAsync(userId).ContinueWith(r =>
            {
                User result = r.Result;
                if (result?.Heroes != null && result.Heroes.Count > 0)
                    heroId = result.Heroes[0].Id;
            });
            await Task.WhenAll(userTask);
            using (IEventStream stream = _store.OpenStream(new Guid(id)))
            {
                stream.Add(new EventMessage
                {
                    Body = new UpdateOpponentInfoEvent
                    {
                        OpponentId = userId,
                        OpponentHeroId = heroId
                    }
                });
                stream.Add(new EventMessage
                {
                    Body = new ChangeGameStatusEvent
                    {
                        Status = GameStatus.Playing
                    }
                });

                stream.CommitChanges(Guid.NewGuid());
            }
            await Repository.AcceptedAsync(id, userId, heroId);
        }

        public async Task GenerateCastlesAsync(string id, GenerateCastleData castles)
        {
            var game = await Build(new Guid(id), string.Empty, -1);
            foreach (CastleModel t in castles.Castles)
            {
                AddCastleEvent addCastleEvent = new AddCastleEvent(t.OwnerUserId);
                t.Id = addCastleEvent.Id.ToString();
                InitCastleEvent init = Mapper.Map<InitCastleEvent>(t);
                init.Id = addCastleEvent.Id;
                init.CreatedBy = init.OwnerUserId;
                var gameId = new Guid(id);
                var createFirstSoldier = _gameDomainService.GetCreateSoldierEvent(gameId, addCastleEvent.Id, t.ProducedTroopTypes.First(),
                    t.OwnerUserId, true);
                var createSoldier = _gameDomainService.GetCreateSoldierEvent(gameId, addCastleEvent.Id, t.ProducedTroopTypes.First(),
                    t.OwnerUserId);
                _domain.AddEvent(gameId, addCastleEvent, init, createFirstSoldier, createSoldier);
            }
            var reveuneEv = _gameDomainService.RevenueCoinEvent(game.Speed);
            reveuneEv.RunningAt = reveuneEv.ExecuteAt = DateTime.UtcNow;
            _domain.AddEvent(game.Id, reveuneEv, _gameDomainService.UpkeepCoinEvent(game.Speed));
            var gameEntity =await _gameRepository.GetByIdAsync(id);
            gameEntity.Castles = castles.Castles.Select(e=>e.Id).ToList();
            gameEntity.Routes = castles.Routes.Select(e=>new CastleRoute()
            {
                FromCastle = e.FromCastle.Id,
                ToCastle = e.ToCastle.Id
            }).ToList();
            await _gameRepository.UpdateAsync(gameEntity);
        }

        public async Task<bool> SuspendCastleProduction(Guid id, Guid castleId)
        {
            var state = await Build(id, string.Empty, -1);
            if (state.HasError)
                return false;
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            var castle = gameSnapshot?.Castles?.FirstOrDefault(e => e.Id == castleId);
            if (castle == null)
                return false;
            _domain.AddEvent(id, new SuspendCastleProductionEvent(castleId, castle.OwnerUserId));
            return true;
        }

        public async Task<bool> RestartCastleProduction(Guid id, Guid castleId)
        {
            var state = await Build(id, string.Empty, -1);
            if (state.HasError)
                return false;
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            var castle = gameSnapshot?.Castles?.FirstOrDefault(e => e.Id == castleId);
            if (castle == null)
                return false;

            if (gameSnapshot.CanProduce(castle,
                _gameDomainService.GetUpkeepCoinBySoldierType(castle, castle.GetDefaultTroopType())))
            {
                _domain.AddEvent(id, new RestartCastleProductionEvent(castleId, castle.OwnerUserId));
                return true;
            }
            return false;
        }

        public async Task<UpgradeCastleResult> UpgradeCastleAsync(Guid id, Guid castleId)
        {
            var state = await Build(id, string.Empty, -1);
            if (state.HasError)
                return new UpgradeCastleResult()
                {
                    Errors = new List<string>()
                    {
                        "Failed when building game state"
                    }
                };
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            var castle = gameSnapshot?.Castles?.FirstOrDefault(e => e.Id == castleId);
            if (castle == null)
                return new UpgradeCastleResult()
                {
                    Errors = new List<string>()
                    {
                        "Castle not found"
                    }
                };

            var maxStrength = await GetMaximunStrength();
            if (castle.Strength < maxStrength)
            {
                _domain.AddEvent(id, new UpgradeCastleEvent(castleId, castle.OwnerUserId));
                return new UpgradeCastleResult()
                {
                    Strength = castle.Strength + 1
                };
            }

            return new UpgradeCastleResult()
            {
                Errors = new List<string>()
                {
                    "Your Castle is got max strength"
                }
            };
        }

        public async Task AddCoinToUserAsync(Guid gameId, string userId, double coin)
        {
            var state = await Build(gameId, string.Empty, -1);
            if (state.HasError)
                return;
            var army = state.UserId == userId ? Army.Blue : Army.Red;
            _domain.AddEvent(gameId, new CollectedCoinEvent(coin, army, userId));
        }

        public async Task<Shared.PagingResult<GameModel>> PagingAsync(Shared.PagingByIdModel model)
        {
            Shared.PagingResult<GameModel> result = new Shared.PagingResult<GameModel>();
            var pagingResult = await Repository.PagingAsync(Mapper.Map<DataAccess.Models.PagingByIdModel>(model));
            if (pagingResult?.Items == null)
                return result;
            result.CanLoadMore = pagingResult.CanLoadMore;
            result.Total = pagingResult.Total;
            result.Items = new List<GameModel>();
            List<Task> tasks = new List<Task>();
            foreach (Game item in pagingResult.Items)
            {
                GameModel game = Mapper.Map<GameModel>(item);
                tasks.Add(Task.WhenAll(
                    _userRepository.GetByIdAsync(game.CreatedBy).ContinueWith(r =>
                    {
                        if (r.Result == null)
                            return;
                        game.User = Mapper.Map<UserModel>(r.Result);
                        List<Hero> heroes = r.Result.Heroes;
                        if (heroes == null || heroes.All(e => e.Id != game.UserHeroId))
                            return;
                        game.UserHero = Mapper.Map<HeroModel>(r.Result.Heroes.First(e => e.Id == game.UserHeroId));
                    }),
                   string.IsNullOrEmpty(game.OpponentId) ? Task.FromResult(true) : _userRepository.GetByIdAsync(game.OpponentId).ContinueWith(r =>
                     {
                         if (r.Result == null)
                             return;
                         game.Opponent = Mapper.Map<UserModel>(r.Result);
                         List<Hero> heroes = r.Result.Heroes;
                         if (heroes == null || heroes.All(e => e.Id != game.OpponentHeroId))
                             return;
                         game.OpponentHero = Mapper.Map<HeroModel>(r.Result.Heroes.First(e => e.Id == game.OpponentHeroId));
                     })).ContinueWith(r => result.Items.Add(game)));
            }
            await Task.WhenAll(tasks);
            return result;
        }

        public async Task<DetailCastleStateModel> DetailCastle(Guid id, Guid castleId, string userId, int streamVersion)
        {
            var state = await Build(id, userId, streamVersion);
            if (state.HasError)
                return null;
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            var castle = gameSnapshot?.Castles?.FirstOrDefault(e => e.Id == castleId);
            if (castle == null)
                return null;
            DetailCastleStateModel result = Mapper.Map<DetailCastleStateModel>(castle);
            result.StreamRevision = latestSnapshot.StreamRevision;
            result.CurrentUserArmy = state.UserId == userId ? Army.Blue : Army.Red;
            if (streamVersion >= 0)
                result.Events = Mapper.Map<List<EventBaseModel>>(_domain.GetEvents(id, userId, streamVersion));
            result.Soldiers = GetSoldiersOfCastle(castle);
            result.CurrentTroopType = castle.GetDefaultTroopType();
            bool isOwner = result.OwnerUserId == userId;
            result.CanChangeTroopType = isOwner;
            result.CanUpgrade = isOwner && result.Strength < await GetMaximunStrength();
            result.AvailableTroopTypes = !result.CanChangeTroopType ?
                new List<CastleTroopTypeModel>() :
                Mapper.Map<List<CastleTroopTypeModel>>(castle.TroopTypes?.Where(e => e.ResourceType != result.CurrentTroopType).ToList());
            await UpdateGameStateForReadData(id.ToString(), state);
            result.Revenue = _gameDomainService.CalculateCoin(gameSnapshot, castle);
            result.RevenueTime = _gameDomainService.GetRevenueTimeBySpeed(gameSnapshot.Speed);
            result.UpkeepTime = _gameDomainService.GetUpkeepTimeBySpeed(gameSnapshot.Speed);
            var productEvent = GetLatestProductionTime(id, castleId);
            if (productEvent == null)
            {
                result.ProduceExecuteAt = default(DateTime);
            }
            else
            {
                result.ProduceExecuteAt = productEvent.ExecuteAt;
                result.ProduceTroopType = productEvent.TroopType;
            }
            result.CanProductionSoldier = castle.IsProductionState() && result.ProduceExecuteAt.CompareTo(DateTime.UtcNow) > 0;
            if (!result.CanProductionSoldier)
            {
                var ownerCoins = castle.OwnerUserId == state.UserId ? state.UserCoins : state.OpponentCoins;
                result.IsNotEnoughCoinForProduction = ownerCoins < 0;
            }
            var ownerTask = _userRepository.GetByIdAsync(castle.OwnerUserId).ContinueWith(r =>
            {
                if (r.Result == null)
                    return;
                result.OwnerUser = Mapper.Map<UserModel>(r.Result);
                List<Hero> heroes = r.Result.Heroes;
                if (heroes == null || heroes.All(e => e.Id != castle.OwnerId))
                    return;
                result.Owner = Mapper.Map<HeroModel>(r.Result.Heroes.First(e => e.Id == castle.OwnerId));
            });
            var siegeOwnerTask = castle.Siege == null ? Task.FromResult(true) : _userRepository.GetByIdAsync(castle.Siege.OwnerUserId).ContinueWith(r =>
               {
                   if (r.Result == null)
                       return;
                   result.Siege.OwnerUser = Mapper.Map<UserModel>(r.Result);
               });
            await Task.WhenAll(ownerTask, siegeOwnerTask);
            return result;
        }

        private Task<double> GetMaximunStrength()
        {
            return Task.FromResult<double>(3);
        }

        private CreateSoldierEvent GetLatestProductionTime(Guid gameId, Guid castleId)
        {
            CreateSoldierEvent latestEvent = _domain.GetLatestEvent<CreateSoldierEvent>(gameId, e => e.CastleId == castleId);
            return latestEvent;
        }

        private List<SoldierModel> GetSoldiersOfCastle(CastleAggregate castle)
        {
            var availableSoldiers = castle.GetAvailableSoldiers();
            var soldiers = availableSoldiers != null ? Mapper.Map<List<SoldierModel>>(availableSoldiers) : new List<SoldierModel>();
            foreach (var soldier in soldiers)
            {
                soldier.UpkeepCoins = _gameDomainService.GetUpkeepCoinBySoldierType(castle, soldier.CastleTroopType.ResourceType);
            }
            return soldiers;
        }

        public async Task<string> BattalionAsync(Guid id, BattalionModel model, RouteModel route, string userId)
        {
            var game = await Repository.GetByIdAsync(id.ToString());
            if (game == null)
                throw new KeyNotFoundException("Game not found");
            var startBattleEvent = new StartBattalionEvent(model.Id, new Guid(model.CastleId), new Guid(model.DestinationCastleId),
                model.Soldiers,
                Mapper.Map<Route>(route),
                userId,
                DateTime.UtcNow, DateTime.UtcNow);
            _domain.AddEvent(id, startBattleEvent);
            return startBattleEvent.Id.ToString();
        }

        public async Task UpdateStreamVersionAsync(string id, string userId, int streamVersion)
        {
            var game = await Repository.GetByIdAsync(id);
            if (game == null)
                return;
            await Repository.UpdateStreamVersionAsync(id, streamVersion, game.CreatedBy != userId);
        }

        public async Task<int> GetStreamVersionAsync(string id, string userId)
        {
            var game = await Repository.GetByIdAsync(id);
            if (game == null)
                return -1;
            if (game.CreatedBy == userId)
                return game.UserStreamVersion;
            return game.OpponentStreamVersion;
        }

        public async Task SetOpponentArmySettingAsync(string id, GameArmySettingModel armySetting)
        {
            var game = await Repository.GetByIdAsync(id);
            if (game == null)
                throw new KeyNotFoundException();
            var opponentArmySetting = Mapper.Map<GameArmySetting>(armySetting);
            using (IEventStream stream = _store.OpenStream(new Guid(id)))
            {
                stream.Add(new EventMessage
                {
                    Body = new SelectOpponentArmySettingEvent
                    {
                        ArmySetting = opponentArmySetting
                    }
                });
            }
            await Repository.SetOpponentArmySettingAsync(id, opponentArmySetting);
        }
        public async Task AcceptedSelfPlayingGameAsync(string id)
        {
            var game = await Repository.GetByIdAsync(id);
            if (game == null)
                throw new KeyNotFoundException();

            using (IEventStream stream = _store.OpenStream(new Guid(id)))
            {
                stream.Add(new EventMessage
                {
                    Body = new ChangeGameStatusEvent
                    {
                        Status = GameStatus.Playing
                    }
                });
                //var reveuneEv = _gameDomainService.RevenueCoinEvent(game.Speed);
                //reveuneEv.RunningAt = reveuneEv.ExecuteAt = DateTime.UtcNow;
                //stream.Add(new EventMessage
                //{
                //    Body = reveuneEv
                //});
                //stream.Add(new EventMessage
                //{
                //    Body = _gameDomainService.UpkeepCoinEvent(game.Speed)
                //});
                stream.CommitChanges(Guid.NewGuid());
            }
            await Repository.AcceptedSelfPlayingAsync(id);
        }

        public async Task<GameModel> GetGameDetailAsync(Guid id)
        {
            var game = await GetByIdAsync(id.ToString());
            await Task.WhenAll(
                _userRepository.GetByIdAsync(game.CreatedBy).ContinueWith(r =>
                {
                    if (r.Result == null)
                        return;
                    game.User = Mapper.Map<UserModel>(r.Result);
                    List<Hero> heroes = r.Result.Heroes;
                    if (heroes == null || heroes.All(e => e.Id != game.UserHeroId))
                        return;
                    game.UserHero = Mapper.Map<HeroModel>(r.Result.Heroes.First(e => e.Id == game.UserHeroId));
                }),
                string.IsNullOrEmpty(game.OpponentId)
                    ? Task.FromResult(true)
                    : _userRepository.GetByIdAsync(game.OpponentId).ContinueWith(r =>
                    {
                        if (r.Result == null)
                            return;
                        game.Opponent = Mapper.Map<UserModel>(r.Result);
                        List<Hero> heroes = r.Result.Heroes;
                        if (heroes == null || heroes.All(e => e.Id != game.OpponentHeroId))
                            return;
                        game.OpponentHero =
                            Mapper.Map<HeroModel>(r.Result.Heroes.First(e => e.Id == game.OpponentHeroId));
                    }));
            return game;
        }

        public async Task SetHeroAroundCastleAsync(HeroAroundCastleModel model)
        {
            var gamesOfUser = await Repository.GetAllPlayingGameOfHero(model.UserId, model.HeroId);
            if (gamesOfUser == null || gamesOfUser.Count == 0)
                return;
            foreach (var g in gamesOfUser)
            {
                var game = await Build(new Guid(g.Id), model.UserId, -1);
                var nearbyCastles =
                    game.Castles?.Where(
                        e => e.Position != null &&
                            Helpers.DistanceBetween(e.Position.Lat, e.Position.Lng, model.Position.Lat,
                                model.Position.Lng) <= _gameSettings.DistanceHeroARoundCastleThreshold).ToList() ?? new List<CastleStateModel>();
                if (!nearbyCastles.Any())
                    continue;
                _domain.AddEvent(game.Id, nearbyCastles.Select(castle => new HeroARoundCastleEvent(new Guid(castle.Id), model.UserId, model.HeroId)).Cast<EventBase>().ToArray());
            }
        }

        public async Task UpdateNearbyHero()
        {
            var games = await Repository.GetAllPlayingGame() ?? new List<Game>();
            if (games.Count == 0)
                return;
            foreach (var g in games)
            {
                var events = new List<EventBase>();
                var game = await Build(new Guid(g.Id), string.Empty, -1);
                foreach (var c in game.Castles)
                {
                    var castle = await DetailCastle(new Guid(g.Id), new Guid(c.Id), string.Empty, -1);
                    // check from last updated time
                    var outOfDateHeros =
                        castle.Heroes?.Where(
                            e =>
                                e.UpdatedAt.Add(TimeSpan.FromMinutes(_gameSettings.HeroStayInCastleTime))
                                    .CompareTo(DateTime.UtcNow) < 0).ToList() ?? new List<HeroStateModel>();
                    if (outOfDateHeros.Count == 0)
                        continue;
                    events.AddRange(outOfDateHeros.Select(hero => new HeroLeftCastleEvent(new Guid(castle.Id), hero.UserId, hero.Id)));
                }
                if (events.Count == 0)
                    continue;
                _domain.AddEvent(new Guid(g.Id), events.ToArray());
            }
        }

#pragma warning disable 1998
        public async Task<GameStateModel> GetState(Guid id, int streamVersion)
#pragma warning restore 1998
        {
            GameStateModel result = new GameStateModel
            {
                Id = id,
                HasError = false
            };
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, streamVersion);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            if (gameSnapshot == null)
            {
                return null;
            }
            result.Status = gameSnapshot.Status;
            result.UserId = gameSnapshot.UserId;
            result.UserHeroId = gameSnapshot.UserHeroId;
            result.OpponentId = gameSnapshot.OpponentId;
            result.OpponentHeroId = gameSnapshot.OpponentHeroId;
            result.StreamRevision = latestSnapshot.StreamRevision;
            result.SelfPlaying = gameSnapshot.SelfPlaying;
            result.Castles = new List<CastleStateModel>();
            foreach (var castle in gameSnapshot.Castles)
            {
                CastleStateModel castleModel = Mapper.Map<CastleStateModel>(castle);
                //var events = _domain.GetNotExecuteEvents<CreateSoldierEvent>(id);
                //var latestEvent = events.FirstOrDefault(e => e.CastleId == castle.Id);
                //if (latestEvent != null)
                //    castleModel.ProduceExecuteAt = latestEvent.ExecuteAt;
                result.Castles.Add(castleModel);
            }
            return result;
        }

        public async Task<bool> CanComputerSendBattalion(Guid gameId)
        {
            var game = await Repository.GetByIdAsync(gameId.ToString());
            var battalionEvents = _domain.GetNotExecuteEvents<BattalionMovementEvent>(gameId)?.Where(e => e.CreatedBy != game.CreatedBy).ToList() ?? new List<BattalionMovementEvent>();
            var siegeEvents = _domain.GetNotExecuteEvents<SiegeCastleEvent>(gameId)?.Where(e => string.IsNullOrEmpty(e.SiegeBy)).ToList() ?? new List<SiegeCastleEvent>();
            if (battalionEvents.Any() || siegeEvents.Any())
                return false;
            return true;
        }

        public GameStateModel GetLatestSnapshot(Guid id)
        {
            GameStateModel result = new GameStateModel()
            {
                Id = id
            };
            _domain.Build(id, InitGameSnapshot);
            result.HasError = false;
            ISnapshot latestSnapshot = _store.Advanced.GetSnapshot(id, int.MaxValue);
            GameAggregate gameSnapshot = latestSnapshot?.Payload as GameAggregate;
            if (gameSnapshot == null)
            {
                result.HasError = true;
                return result;
            }
            result = GetGameStateBySnapshot(gameSnapshot);
            result.StreamRevision = latestSnapshot.StreamRevision;
            return result;
        }

        public Task RemoveAsync(Guid id)
        {
            return Repository.RemoveAsync(id.ToString());
        }

        private GameStateModel GetGameStateBySnapshot(GameAggregate gameSnapshot)
        {
            GameStateModel result = new GameStateModel
            {
                Id = gameSnapshot.Id,
                Status = gameSnapshot.Status,
                UserId = gameSnapshot.UserId,
                UserHeroId = gameSnapshot.UserHeroId,
                OpponentId = gameSnapshot.OpponentId,
                OpponentHeroId = gameSnapshot.OpponentHeroId,
                SelfPlaying = gameSnapshot.SelfPlaying,
                Speed = gameSnapshot.Speed,
                Difficulty = gameSnapshot.Difficulty,
                UserCoins = gameSnapshot.UserCoins,
                OpponentCoins = gameSnapshot.OpponentCoins,
                Castles = new List<CastleStateModel>()
            };
            foreach (var castle in gameSnapshot.Castles)
            {
                CastleStateModel castleModel = Mapper.Map<CastleStateModel>(castle);
                var events = _domain.GetNotExecuteEvents<CreateSoldierEvent>(gameSnapshot.Id);
                var latestEvent = events.FirstOrDefault(e => e.CastleId == castle.Id);
                if (latestEvent != null)
                    castleModel.ProduceExecuteAt = latestEvent.ExecuteAt;
                result.Castles.Add(castleModel);
            }
            return result;
        }

        public bool IsBattalionMovementExecuted(CheckBattalionMovementEventModel model)
        {
            return _domain.IsEventExecuted<BattalionMovementEvent>(model.GameId, model.StreamVersion, model.EventId);
        }

        public async Task ChangeTroopType(ChangeTroopTypeDto model)
        {
            var game = await Repository.GetByIdAsync(model.Id);
            if (game == null)
                throw new KeyNotFoundException("Game not found");
            _domain.AddEvent(new Guid(model.Id), new ChangeCastleTroopTypeEvent(new Guid(model.CastleId), model.TroopType));
        }

        public async Task<List<GameModel>> GetAllAsync()
        {
            var items = await Repository.Collection.Find(Builders<Game>.Filter.Ne(e => e.IsRemoved, true)).ToListAsync() ?? new List<Game>();
            return Mapper.Map<List<GameModel>>(items);
        }

        public Task<bool> OccupiedArtifactAsync(string id, string userId, string occupiedId)
        {
            return Task.FromResult(true);
        }
    }
}

