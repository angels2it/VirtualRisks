using CastleGo.Application.Games;
using CastleGo.Application.Users;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CastleGo.Application.CastleTroopTypes;
using CastleGo.Application.Games.Dtos;
using CastleGo.Application.Settings;
using CastleGo.Application.Settings.Dtos;
using CastleGo.Domain;
using CastleGo.GameAi;
using GoogleMapsApi.Entities.Directions.Response;
using Hangfire;
using Microsoft.AspNet.Identity;
using CastleGo.Providers;
using CastleGo.WebApi.Directions;
using CastleGo.WebApi.Models;
using SmartFormat;
using Swashbuckle.Swagger.Annotations;

namespace CastleGo.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix(Startup.ApiPrefix + "/game")]
    public class GameController : ProtectedController
    {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly IDirectionProvider _directionProvider;
        private readonly GameSettings _gameSettings;
        private readonly GameAiSettings _gameAiSettings;
        private readonly NotifySettings _notifySettings;
        private readonly ICastleTroopTypeService _castleTroopTypeService;
        private readonly IGameArmySettingService _armySettingService;
        private readonly ICastleDirectionService _directionService;

        static Random R = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameService"></param>
        /// <param name="userService"></param>
        /// <param name="directionProvider"></param>
        /// <param name="gameSettings"></param>
        /// <param name="gameAiSettings"></param>
        /// <param name="notifySettings"></param>
        /// <param name="castleTroopTypeService"></param>
        /// <param name="armySettingService"></param>
        public GameController(IGameService gameService, IUserService userService, IDirectionProvider directionProvider, GameSettings gameSettings, GameAiSettings gameAiSettings, NotifySettings notifySettings, ICastleTroopTypeService castleTroopTypeService, IGameArmySettingService armySettingService, ICastleDirectionService directionService)
        {
            _gameService = gameService;
            _userService = userService;
            _directionProvider = directionProvider;
            _gameSettings = gameSettings;
            _gameAiSettings = gameAiSettings;
            _notifySettings = notifySettings;
            _castleTroopTypeService = castleTroopTypeService;
            _armySettingService = armySettingService;
            _directionService = directionService;
            R = new Random();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Paging")]
        public async Task<IHttpActionResult> Paging(int page, int take)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId) || !await _userService.IsUserActivatedAsync(userId))
                return Unauthorized();
            PagingResult<GameModel> content = await _gameService.PagingAsync(new PagingByIdModel()
            {
                Page = page,
                Take = take,
                Id = userId
            });
            return Ok(content);
        }

        /// <summary>
        /// Remove game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Remove(string id)
        {
            var game = _gameService.GetLatestSnapshot(new Guid(id));
            if (game == null)
                return NotFound();
            var userId = User.Identity.GetUserId();
            if (game.OpponentId != userId && game.UserId != userId)
                return NotFound();
            await _gameService.RemoveAsync(new Guid(id));
            RecurringJob.RemoveIfExists(id);
            return Ok(new RemoveGameModelResult
            {
                Id = id
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="streamVersion"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Build")]
        [SwaggerResponse(200, type: typeof(GameStateModel))]
        public async Task<IHttpActionResult> Build(string id, int streamVersion = 0)
        {
            GameStateModel content = await _gameService.Build(new Guid(id), User.Identity.GetUserId(), streamVersion);
            return Ok(content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="streamVersion"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("State")]
        public async Task<IHttpActionResult> GetGame(string id, int streamVersion)
        {
            GameStateModel content = await _gameService.GetState(new Guid(id), streamVersion);
            return Ok(content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/Info")]
        [SwaggerResponse(200, type: typeof(GameModel))]
        public async Task<IHttpActionResult> Info(string id)
        {
            return Ok(await _gameService.GetGameDetailAsync(new Guid(id)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        [Swashbuckle.SwaggerGen.Annotations.SwaggerOperation("CreateGame")]
        [SwaggerResponse(200, type: typeof(CreateGameModelResult))]
        public async Task<IHttpActionResult> Create(CreateGameModel model)
        {
            if (!model.SelfPlaying && string.IsNullOrEmpty(model.OpponentId) && model.OpponentExtInfo == null)
                return InternalServerError(new Exception("Not engouh info"));
            string userId = User.Identity.GetUserId();
            try
            {
                var gameId = await _gameService.CreateAsync(userId, model);
                // if self-playing - auto accept game
                if (model.SelfPlaying)
                {
                    await Accepted(gameId, null);
                    RecurringJob.AddOrUpdate<IGameAiFactory>(gameId, p => p.AnayticForGame(gameId), Cron.MinuteInterval(_gameAiSettings.Interval));
                }
                if (!string.IsNullOrEmpty(model.OpponentId) || !string.IsNullOrEmpty(model.OpponentExtInfo?.Key))
                {
                    var opponent = await _userService.GetByIdAsync(model.OpponentId) ??
                                   await _userService.GetBySocialInfoAsync(model.OpponentExtInfo);
                    var user = await _userService.GetByIdAsync(userId);
                    if (opponent != null && user != null)
                    {
                        var inviteModel = new NotifyGameInviteModel
                        {
                            CurrentUser = opponent,
                            Opponent = user
                        };
                        BackgroundJob.Enqueue<INotifySenderProvider>(
                               a =>
                                   a.SendNotify(Mapper.Map<List<NotifyTokenDto>>(opponent.Tokens),
                                   gameId,
                                   "GameInvite", "icon",
                                   Smart.Format(_notifySettings.GameInviteTitle, inviteModel),
                                   Smart.Format(_notifySettings.GameInviteMessage, inviteModel)));
                    }
                }
                return Ok(new CreateGameModelResult
                {
                    Id = gameId
                });
            }
            catch (KeyNotFoundException ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/Castle")]
        [SwaggerResponse(200, type: typeof(DetailCastleStateModel))]
        public async Task<IHttpActionResult> Castle(string id, string castleId, int streamVersion)
        {
            DetailCastleStateModel content = await _gameService.DetailCastle(new Guid(id), new Guid(castleId), User.Identity.GetUserId(), streamVersion);
            return Ok(content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Battalion")]
        [SwaggerResponse(200, type: typeof(string))]
        public async Task<IHttpActionResult> Battalion(string id, BattalionModel model)
        {
            var game = await _gameService.Build(new Guid(id), User.Identity.GetUserId(), -1);
            if (game == null || game.HasError)
                return NotFound();
            var castle = game.Castles?.FirstOrDefault(e => e.Id == model.CastleId);
            var destinationCastle = game.Castles?.FirstOrDefault(e => e.Id == model.DestinationCastleId);
            if (castle == null || destinationCastle == null)
                return NotFound();
            var userId = User.Identity.GetUserId();
            if (castle.OwnerUserId != userId)
            {
                return BadRequest("You don't have permission to performance this action on the castle");
            }
            if (model.MoveByPercent)
            {
                model.Soldiers = await AutoSelectSoldiers(game.Id, new Guid(castle.Id), model.PercentOfSelectedSoldiers);
                if (!model.Soldiers.Any())
                    return NotFound();
            }
            // get route
            RouteModel route = await GetBattalionRoute(game, castle, destinationCastle, model.Soldiers);
            if (route == null)
                return InternalServerError(new Exception("Can not get route between two castle!"));
            return Ok(await _gameService.BattalionAsync(game.Id, model, route, User.Identity.GetUserId()));
        }

        private async Task<List<string>> AutoSelectSoldiers(Guid gameId, Guid castleId, int percentOfSelectedSoldiers)
        {
            var castleInfo = await _gameService.DetailCastle(gameId, castleId, string.Empty, -1);
            if (castleInfo?.Soldiers == null)
                return new List<string>();
            switch (percentOfSelectedSoldiers)
            {
                case 50:
                    var take = (int)Math.Ceiling((double)castleInfo.Soldiers.Count / 2);
                    return castleInfo.Soldiers.Take(take).Select(e => e.Id).ToList();
                default:
                    return castleInfo.Soldiers.Select(e => e.Id).ToList();
            }
        }

        /// <summary>
        /// Change troop type of castle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/trooptype")]
        public async Task<IHttpActionResult> ChangeTroopType(string id, ChangeTroopTypeModel model)
        {
            await _gameService.ChangeTroopType(new ChangeTroopTypeDto
            {
                Id = id,
                CastleId = model.CastleId,
                TroopType = model.Type
            });
            return Ok(true);
        }

        /// <summary>
        /// Get route between two castle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("route")]
        public async Task<IHttpActionResult> Route(BattalionModel model)
        {
            var game = _gameService.GetLatestSnapshot(model.Id);
            if (game == null)
                return NotFound();
            var castle = game.Castles?.FirstOrDefault(e => e.Id == model.CastleId);
            var destinationCastle = game.Castles?.FirstOrDefault(e => e.Id == model.DestinationCastleId);
            if (castle == null || destinationCastle == null)
                return NotFound();
            if (model.MoveByPercent)
            {
                model.Soldiers = await AutoSelectSoldiers(game.Id, new Guid(castle.Id), model.PercentOfSelectedSoldiers);
                if (!model.Soldiers.Any())
                    return NotFound();
            }
            var route = await GetBattalionRoute(game, castle, destinationCastle, model.Soldiers);
            if (route == null)
                return InternalServerError(new Exception("Can not get route between two castle!"));
            return Ok(route);
        }

        private async Task<RouteModel> GetBattalionRoute(GameStateModel game, CastleStateModel castle, CastleStateModel destinationCastle, List<string> soldierIds)
        {
            // get route
            RouteModel route = null;
            var detailCastle = await _gameService.DetailCastle(game.Id, new Guid(castle.Id), string.Empty, -1);
            var soldiers = detailCastle.Soldiers?.Where(e => soldierIds.Contains(e.Id)).ToList();
            if (soldiers == null)
                return null;
            var gameSpeed = _gameSettings.GetMovementSpeedOfGame(
                GameSpeedHelper.GetSpeedValue(game.Speed));
            var battalionSpeed = soldiers.Min(e => e.CastleTroopType.MovementSpeed) * gameSpeed;
            var isNotFight = soldiers.Any(e => !e.CastleTroopType.IsFlight);
            if (isNotFight)
            {
                var direction = await _directionProvider.GetDirection(castle.Position, destinationCastle.Position);
                if (direction.Status == DirectionsStatusCodes.OK && direction.Routes != null && direction.Routes.Any() &&
                    direction.Routes.ElementAt(0).Legs != null && direction.Routes.ElementAt(0).Legs.Any())
                {
                    var selectedRouteLeg = direction.Routes.ElementAt(0).Legs.ElementAt(0);
                    route = new RouteModel
                    {
                        Steps = selectedRouteLeg.Steps.Select(step => new RouteStepModel()
                        {
                            StartLocation =
                                new PositionModel
                                {
                                    Lat = step.StartLocation.Latitude,
                                    Lng = step.StartLocation.Longitude
                                },
                            EndLocation =
                                new PositionModel { Lat = step.EndLocation.Latitude, Lng = step.EndLocation.Longitude },
                            Distance = step.Distance.Value,
                            Duration =
                                TimeSpan.FromSeconds(step.Distance.Value /
                                                     battalionSpeed)
                        }).ToList(),
                        Distance = selectedRouteLeg.Distance.Value,
                        Duration =
                            TimeSpan.FromSeconds(selectedRouteLeg.Distance.Value /
                                                 battalionSpeed)
                    };
                }
            }
            else
            {
                var distance = Helpers.DistanceBetween(castle.Position.Lat, castle.Position.Lng,
                    destinationCastle.Position.Lat, destinationCastle.Position.Lng);
                var duration = TimeSpan.FromSeconds(distance / battalionSpeed);
                route = new RouteModel()
                {
                    Distance = distance,
                    Duration = duration,
                    Steps = new List<RouteStepModel>()
                    {
                        new RouteStepModel()
                        {
                            StartLocation = castle.Position,
                            EndLocation = destinationCastle.Position,
                            Distance = distance,
                            Duration = duration
                        }
                    }
                };
            }

            return route;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}/StreamVersion")]
        public async Task<IHttpActionResult> GetStreamVersion(string id)
        {
            return Ok(await _gameService.GetStreamVersionAsync(id, User.Identity.GetUserId()));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="streamVersion"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/StreamVersion")]
        public async Task<IHttpActionResult> UpdateStreamVersion(string id, int streamVersion)
        {
            await _gameService.UpdateStreamVersionAsync(id, User.Identity.GetUserId(), streamVersion);
            return Ok(streamVersion);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IsBattalionMovementExecuted")]
        public IHttpActionResult IsBattalionMovementExecuted(CheckBattalionMovementEventModel model)
        {
            return Ok(_gameService.IsBattalionMovementExecuted(model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Accepted")]
        public async Task<IHttpActionResult> Accepted(string id, GameAcceptedModel model)
        {
            GameModel game = await _gameService.GetByIdAsync(id);
            if (game == null)
                return NotFound();
            if (game.Status != GameStatus.Requesting)
                return BadRequest("Game status must be requesting");
            // get castles
            List<PositionModel> curList = new List<PositionModel>();
            List<PositionModel> result = await GetNearByPlaces(curList, game.Position.Lat, game.Position.Lng, string.Empty);
            if (result == null)
                return InternalServerError(new Exception("Can not contact to map service"));
            if (result.Count < 20)
            {
                curList = new List<PositionModel>();
                result = await GetNearByPlaces(curList, game.Position.Lat, game.Position.Lng, string.Empty, 0, 1000);
            }
            if (result.Count < 20)
                return InternalServerError(new Exception("Error.NotEnoughVincityPlaceToPlay"));



            var opponentArmySetting = game.SelfPlaying
                ? await GetOpponentArmySettingForSelfPlayingGame(game) : model.ArmySetting;
            await _gameService.SetOpponentArmySettingAsync(id, opponentArmySetting);
            game.OpponentArmySetting = opponentArmySetting;
            var castlesData = game.SelfPlaying ?
               await GenerateCastleForSelfPlayingGame(result, game) :
                await GenerateCastleForNormalGame(result, game);
            // get user info & accept game
            string userId = User.Identity.GetUserId();
            if (game.CreatedBy != userId)
            {
                UserDto opponent = await _userService.GetByIdAsync(userId);
                if (opponent == null)
                    return Unauthorized();
                if (opponent.Heroes == null || opponent.Heroes.Count == 0)
                    return NotFound();
                game.OpponentId = opponent.Id;
                game.OpponentHeroId = opponent.Heroes[0].Id;
                await _gameService.AcceptedAsync(id, userId, game.OpponentHeroId);
            }
            else
            {
                if (!game.SelfPlaying)
                    return BadRequest("This game is not self-playing mode");
                await _gameService.AcceptedSelfPlayingGameAsync(id);
            }
            // save castle
            await _gameService.GenerateCastlesAsync(id, castlesData);
            return Ok(game.Id);
        }

        private async Task<GameArmySettingModel> GetOpponentArmySettingForSelfPlayingGame(GameModel game)
        {
            var armies = await _armySettingService.GetAllAsync();
            var armiesSelectable = armies.Where(e => e.Id != game.UserArmySetting.Id).ToList();
            var index = R.Next(armiesSelectable.Count);
            return armiesSelectable[index];
        }

        private async Task<GenerateCastleData> GenerateCastleForNormalGame(List<PositionModel> result, GameModel game)
        {
            var cur = new PositionModel()
            {
                Lat = game.Position.Lat,
                Lng = game.Position.Lng
            };
            List<CastleModel> piecesResult = new List<CastleModel>();
            int total = result.Count;
            Dictionary<Army, int> armies = new Dictionary<Army, int>() { { Army.Red, 7 }, { Army.Blue, 7 }, { Army.Neutrual, 6 } };
            for (int i = 0; i < 20; ++i)
            {
                int index;
                do
                {
                    index = R.Next(total);
                }
                while (!CanBeACastle(piecesResult, result[index], cur));
                Army army;
                do
                {
                    army = armies.ElementAt(R.Next(3)).Key;
                }
                while (armies[army] == 0);
                piecesResult.Add(new CastleModel
                {
                    Position = result[index],
                    Name = GetCastleName(game, army, piecesResult.Count(e => e.Army == army)),
                    Army = army,
                    MaxResourceLimit = 10,
                    OwnerId = GetCastleHeroOwnerIdByArmy(army, game),
                    OwnerUserId = GetCastleOwnerIdByArmy(army, game),
                    TroopTypes = await GetRandomTroopTypes(),
                    ProducedTroopTypes = new List<string>()
                    {
                       await GetRandomTroopType()
                    },
                    Strength = _gameSettings.WallStrength,
                });
                armies[army] = armies[army] - 1;
            }
            return new GenerateCastleData()
            {
                Castles = piecesResult
            };
        }

        private async Task<string> GetRandomTroopType()
        {
            var troopTypes = await _castleTroopTypeService.GetAllAsync();
            var typeNum = R.Next(troopTypes.Count);
            return troopTypes[typeNum].ResourceType;
        }

        private async Task<GenerateCastleData> GenerateCastleForSelfPlayingGame(List<PositionModel> result, GameModel game)
        {
            var cur = new PositionModel()
            {
                Lat = game.Position.Lat,
                Lng = game.Position.Lng
            };
            int total = result.Count;
            var castleResult = new List<CastleModel>();
            Dictionary<Army, int> armies = new Dictionary<Army, int>() { { Army.Blue, 10 }, { Army.Red, 10 } };
            for (int i = 0; i < 20; ++i)
            {
                int index;
                do
                {
                    index = R.Next(total);
                }
                while (!CanBeACastle(castleResult, result[index], cur));
                Army army;
                do
                {
                    army = armies.ElementAt(R.Next(2)).Key;
                }
                while (armies[army] == 0);
                var castle = new CastleModel
                {
                    Index = castleResult.Count,
                    Name = GetCastleName(game, army, castleResult.Count(e => e.Army == army)),
                    Position = result[index],
                    Army = army,
                    MaxResourceLimit = 10,
                    OwnerId = GetCastleHeroOwnerIdByArmy(army, game),
                    OwnerUserId = GetCastleOwnerIdByArmy(army, game),
                    TroopTypes = await GetRandomTroopTypes(),
                    Strength = _gameSettings.WallStrength,
                    IsAdded = true
                };
                castle.ProducedTroopTypes = new List<string>()
                {
                    castle.TroopTypes.First(e => e.UpkeepCoins == castle.TroopTypes.Min(f => f.UpkeepCoins)).ResourceType
                };
                castleResult.Add(castle);
                armies[army] = armies[army] - 1;
            }
            // generate route
            var locations = _directionService.GetDirection(cur, castleResult.Select(e=>e.Position).ToList());
            var routes = new List<CastleRouteModel>();
            for (int i = 1; i < locations.Count; i++)
            {
                routes.Add(new CastleRouteModel(castleResult[i - 1], castleResult[i]));
                castleResult[i - 1].RouteCount++;
                castleResult[i].RouteCount++;
            }
            routes.Add(new CastleRouteModel(castleResult[0], castleResult[castleResult.Count - 1]));
            castleResult[0].RouteCount++;
            castleResult[castleResult.Count - 1].RouteCount++;
            var min = 0;
            var max = 18;
            while (max - min > 1)
            {
                routes.Add(new CastleRouteModel(castleResult[min], castleResult[max]));
                castleResult[min].RouteCount++;
                castleResult[max].RouteCount++;
                min++;
                max--;
            }

            // get route
            foreach (var route in routes)
            {
                var r = await GetCastleRoute(route.FromCastle, route.ToCastle);
                if (r == null)
                    throw new Exception($"Can not found route from castle {route.FromCastle.Index} to castle {route.ToCastle.Index}");
                route.Route = r;
            }
            return new GenerateCastleData()
            {
                Castles = castleResult,
                Routes = routes
            };
        }

        private async Task<RouteModel> GetCastleRoute(CastleModel fromCastle, CastleModel toCastle)
        {
            var direction = await _directionProvider.GetDirection(fromCastle.Position, toCastle.Position);
            if (direction.Status == DirectionsStatusCodes.OK && direction.Routes != null && direction.Routes.Any() &&
                direction.Routes.ElementAt(0).Legs != null && direction.Routes.ElementAt(0).Legs.Any())
            {
                var selectedRouteLeg = direction.Routes.ElementAt(0).Legs.ElementAt(0);
                return new RouteModel
                {
                    Steps = selectedRouteLeg.Steps.Select(step => new RouteStepModel()
                    {
                        StartLocation =
                            new PositionModel
                            {
                                Lat = step.StartLocation.Latitude,
                                Lng = step.StartLocation.Longitude
                            },
                        EndLocation =
                            new PositionModel { Lat = step.EndLocation.Latitude, Lng = step.EndLocation.Longitude },
                        Distance = step.Distance.Value
                    }).ToList(),
                    Distance = selectedRouteLeg.Distance.Value
                };
            }
            return null;
        }

        private string GetCastleName(GameModel game, Army army, int index)
        {
            var name = string.Empty;
            switch (army)
            {
                case Army.Red:
                    name = game.OpponentArmySetting?.Castles.Count > index
                        ? game.OpponentArmySetting.Castles[index].Name
                        : string.Empty;
                    break;
                case Army.Blue:
                    name = game.UserArmySetting?.Castles.Count > index
                        ? game.UserArmySetting.Castles[index].Name
                        : string.Empty;
                    break;
            }
            if (string.IsNullOrEmpty(name))
                return $"Castle {index + 1}";
            return name;
        }

        private async Task<List<CastleTroopTypeModel>> GetRandomTroopTypes()
        {
            var types = R.Next(2, 5);
            var troopTypes = new List<CastleTroopTypeModel>();
            for (int i = 0; i < types; i++)
            {
                string troopType;
                do
                {
                    troopType = await GetRandomTroopType();
                } while (troopTypes.Any(e => e.ResourceType == troopType));
                var troopTypeBaseData = await _castleTroopTypeService.GetByTypeAsync(troopType);
                if (troopTypeBaseData == null)
                    continue;
                troopTypes.Add(new CastleTroopTypeModel()
                {
                    ResourceType = troopTypeBaseData.ResourceType,
                    Health = R.Next(troopTypeBaseData.MinHealth, troopTypeBaseData.MaxHealth),
                    AttackStrength = R.Next(troopTypeBaseData.MinAttackStrength, troopTypeBaseData.MaxAttackStrength),
                    MovementSpeed = R.Next(troopTypeBaseData.MinMovementSpeed, troopTypeBaseData.MaxMovementSpeed),
                    ProductionSpeed = TimeSpan.FromMinutes(R.Next(troopTypeBaseData.MinProductionSpeed, troopTypeBaseData.MaxProductionSpeed)),
                    UpkeepCoins = R.Next(troopTypeBaseData.MinUpkeepCoins, troopTypeBaseData.MaxUpkeepCoins),
                    IsFlight = troopTypeBaseData.IsFlight,
                    IsOverComeWalls = troopTypeBaseData.IsOverComeWalls,
                    Icon = troopTypeBaseData.Icon,
                    RedArmyIcon = troopTypeBaseData.RedArmyIcon,
                    BlueArmyIcon = troopTypeBaseData.BlueArmyIcon
                });
            }
            return troopTypes;
        }

        private string GetCastleOwnerIdByArmy(Army army, GameModel game)
        {
            switch (army)
            {
                case Army.Red:
                    return game.OpponentId;
                case Army.Blue:
                    return game.CreatedBy;
                default:
                    return string.Empty;
            }
        }

        private string GetCastleHeroOwnerIdByArmy(Army army, GameModel game)
        {
            switch (army)
            {
                case Army.Red:
                    return game.OpponentHeroId;
                case Army.Blue:
                    return game.UserHeroId;
                default:
                    return string.Empty;
            }
        }

        private bool CanBeACastle(List<CastleModel> curlist, PositionModel item, PositionModel center)
        {
            if (curlist.Any(e => e.Position == item && e.IsAdded))
                return false;
            double num = Helpers.DistanceBetween(item.Lat, item.Lng, center.Lat, center.Lng);
            return num <= 500.0 && num >= 50.0 && !curlist.Any(p => Helpers.DistanceBetween(p.Position.Lat, p.Position.Lng, item.Lat, item.Lng) < 50.0);
        }

        private async Task<List<PositionModel>> GetNearByPlaces(List<PositionModel> curlist, double lat, double lng, string pageToken = "", int curItemCount = 0, int radius = 500)
        {
            //    return new List<PositionModel>()
            //{
            //        new PositionModel(10.78761, 106.6987),
            //    new PositionModel(10.78739,106.69848),
            //    new PositionModel(10.78698,106.69809),
            //    new PositionModel(10.78646,106.69762),
            //    new PositionModel(10.78646,106.69869),
            //    new PositionModel(10.78551,106.69867),
            //    new PositionModel(10.78487,106.69932),
            //    new PositionModel(10.78454,106.69966),
            //    new PositionModel(10.78454,106.69972),
            //    new PositionModel(10.78506,106.70013),
            //    new PositionModel(10.78509,106.70016),
            //    new PositionModel(10.78528,106.70034),
            //    new PositionModel(10.78544,106.7005),
            //    new PositionModel(10.78581,106.70084),
            //    new PositionModel(10.78604,106.70109),
            //    new PositionModel(10.78644,106.70149),
            //    new PositionModel(10.78644,106.70182),
            //    new PositionModel(10.78654,106.70162),
            //    new PositionModel(10.78659,106.70157),
            //    new PositionModel(10.78784,106.70023)
            //};
            PlacesNearByRequest request = new PlacesNearByRequest();
            string appSetting = ConfigurationManager.AppSettings["GoogleApiKey"];
            request.ApiKey = appSetting;
            Location location = new Location(lat, lng);
            request.Location = location;
            double? nullable = radius;
            request.Radius = nullable;
            string str = pageToken;
            request.PageToken = str;
            PlacesNearByResponse placesNearByResponse = await GoogleMaps.PlacesNearBy.QueryAsync(request);
            PlacesNearByResponse response = placesNearByResponse;
            List<PositionModel> result = new List<PositionModel>();
            int triedNumber = 0;
            if (response.Status == Status.OK)
            {
                foreach (Result result1 in response.Results)
                {
                    Result item = result1;
                    if (!string.IsNullOrEmpty(item.Vicinity))
                    {
                        PositionModel pos = new PositionModel() { Lat = item.Geometry.Location.Latitude, Lng = item.Geometry.Location.Longitude, Address = item.Vicinity };
                        result.Add(pos);
                        curlist.Add(pos);
                    }
                }
                if (!string.IsNullOrEmpty(response.NextPage) && curItemCount < 100)
                {
                    List<PositionModel> positionModelList = await GetNearByPlaces(curlist, lat, lng, response.NextPage, curItemCount);
                    List<PositionModel> nextList = positionModelList;
                    if (nextList == null)
                        return null;
                    result.AddRange(nextList);
                    curlist.AddRange(nextList);
                }
            }
            else
            {
                if (response.Status == Status.OVER_QUERY_LIMIT || response.Status == Status.REQUEST_DENIED || response.Status == Status.ZERO_RESULTS)
                    return null;
                if (triedNumber < 3)
                {
                    List<PositionModel> positionModelList = await GetNearByPlaces(curlist, lat, lng, pageToken, curItemCount);
                    List<PositionModel> nextList = positionModelList;
                    if (nextList == null)
                        return null;
                    result.AddRange(nextList);
                    curlist.AddRange(nextList);
                }
            }
            return result;
        }

        /// <summary>
        /// Suspend troop production
        /// </summary>
        /// <param name="id">GameId</param>
        /// <param name="castleId">CastleId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Castles/{castleId}/ProductionStates/Suspend")]
        public async Task<IHttpActionResult> SuspendCastleProduction(string id, string castleId)
        {
            var suspendTask = _gameService.SuspendCastleProduction(new Guid(id), new Guid(castleId));
            return Ok(await suspendTask);
        }
        /// <summary>
        /// Suspend troop production
        /// </summary>
        /// <param name="id">GameId</param>
        /// <param name="castleId">CastleId</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Castles/{castleId}/ProductionStates/Restart")]
        public async Task<IHttpActionResult> RestartCastleProduction(string id, string castleId)
        {
            var restartTask = _gameService.RestartCastleProduction(new Guid(id), new Guid(castleId));
            return Ok(await restartTask);
        }
        /// <summary>
        /// Upgrade castle strength
        /// </summary>
        /// <param name="id"></param>
        /// <param name="castleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Castles/{castleId}/Upgrade")]
        public async Task<IHttpActionResult> UpgradeCastle(string id, string castleId)
        {
            return Ok(await _gameService.UpgradeCastleAsync(new Guid(id), new Guid(castleId)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artifactId"></param>
        /// <returns></returns>
        [HttpPost, Route("game/{id}/Artifacts/{artifactId}/Occupied")]
        public async Task<IHttpActionResult> OccupiedArtifact(string id, string artifactId)
        {
            return Ok(await _gameService.OccupiedArtifactAsync(id, User.Identity.GetUserId(), artifactId));
        }
    }
}
