using CastleGo.Shared;
using CastleGo.Shared.Games;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CastleGo.Application.Games.Dtos;
using CastleGo.Application.Settings.Dtos;
using CastleGo.Entities;

namespace CastleGo.Application.Games
{
    public interface IGameService : IBaseService<GameModel>
    {
        Task<GameStateModel> Build(Guid id, string userId, int streamVersion);
        Task<string> CreateAsync(string userId, CreateGameModel model);
        Task AcceptedAsync(string id, string userId, string heroId = "");
        Task GenerateCastlesAsync(string id, GenerateCastleData castles);
        Task<PagingResult<GameModel>> PagingAsync(PagingByIdModel model);
        Task<DetailCastleStateModel> DetailCastle(Guid id, Guid castleId, string userId, int streamVersion);
        Task<string> BattalionAsync(Guid id, BattalionModel model, RouteModel route, string userId);
        Task UpdateStreamVersionAsync(string id, string userId, int streamVersion);
        Task<int> GetStreamVersionAsync(string id, string userId);
        Task SetOpponentArmySettingAsync(string id, GameArmySettingModel armySetting);
        Task AcceptedSelfPlayingGameAsync(string id);
        Task<GameModel> GetGameDetailAsync(Guid id);
        Task SetHeroAroundCastleAsync(HeroAroundCastleModel model);
        Task UpdateNearbyHero();
        Task<GameStateModel> GetState(Guid id, int streamVersion);
        Task<bool> CanComputerSendBattalion(Guid gameId);
        GameStateModel GetLatestSnapshot(Guid id);
        Task RemoveAsync(Guid id);
        bool IsBattalionMovementExecuted(CheckBattalionMovementEventModel model);
        Task ChangeTroopType(ChangeTroopTypeDto model);
        Task<bool> SuspendCastleProduction(Guid id, Guid castleId);
        Task<bool> RestartCastleProduction(Guid id, Guid castleId);
        Task<UpgradeCastleResult> UpgradeCastleAsync(Guid id, Guid castleId);
        Task AddCoinToUserAsync(Guid gameId, string userId, double coin);
        Task<List<GameModel>> GetAllAsync();
        Task<bool> OccupiedArtifactAsync(string id, string userId, string occupiedId);
        Task<object> MoveSoldierAsync(Guid gameId, MoveSoldierModel model, string userId);
    }
}
