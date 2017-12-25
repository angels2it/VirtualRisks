using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CastleGo.Application.Games;
using CastleGo.Common.Modules;
using CastleGo.Entities;
using CastleGo.Providers;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;

namespace CastleGo.GameAi
{
    public class AttackingGameAiService : IAttackingGameAiService
    {
        static readonly Random R = new Random();
        private readonly ISettingsReader _settingsReader;
        private GameDifficultySettings _gameDifficultySettings;
        private readonly IGameService _gameService;
        private readonly IDirectionProvider _directionProvider;

        public AttackingGameAiService(IGameService gameService, IDirectionProvider directionProvider, ISettingsReader settingsReader)
        {
            _gameService = gameService;
            _directionProvider = directionProvider;
            _settingsReader = settingsReader;
        }

        public async Task Battalion(GameStateModel game)
        {
            InitGameDifficultySetting(game);
            var canBattalion = await _gameService.CanComputerSendBattalion(game.Id);
            if (!canBattalion)
                return;
            // get list cumputer castle
            var castles = game.Castles?.Where(e => e.Army == Army.Red);
            // opponent castle
            var opponentCastle = game.Castles?.Where(e => e.Army == Army.Blue).ToList() ?? new List<CastleStateModel>();
            if (opponentCastle.Count == 0)
                return;
            var canBattalionCastle = castles?.Where(e => e.SoldiersAmount >= _gameDifficultySettings.NumberOfSoldierToBattalion).Take(_gameDifficultySettings.NumberOfCastleToBattalion).ToList() ?? new List<CastleStateModel>();
            if (canBattalionCastle.Count == 0)
            {
                Console.WriteLine("No any castle can battalion");
                return;
            }
            foreach (var castle in canBattalionCastle)
            {
                var detail = await _gameService.DetailCastle(game.Id, new Guid(castle.Id), string.Empty, -1);
                if (detail == null)
                    continue;
                var soldiers = detail.Soldiers.Take(_gameDifficultySettings.NumberOfSoldierToBattalion);
                // battaliton
                var destinationCastle = opponentCastle[R.Next(opponentCastle.Count)];
                // get route
                RouteModel route = await _directionProvider.GetFirstRoute(castle.Position, destinationCastle.Position, game.Speed);
                if (route == null)
                    return;
                await _gameService.BattalionAsync(game.Id, new BattalionModel
                {
                    CastleId = castle.Id,
                    DestinationCastleId = destinationCastle.Id,
                    Id = Guid.NewGuid(),
                    Soldiers = soldiers.Select(e => e.Id).ToList()
                }, route, string.Empty);
            }
        }

        private void InitGameDifficultySetting(GameStateModel game)
        {
            switch (game.Difficulty)
            {
                case GameDifficulfy.Easy:
                    _gameDifficultySettings = (GameDifficultySettings)_settingsReader.Load(typeof(GameDifficultySettings), "GameDifficultyEasy");
                    break;
                case GameDifficulfy.Normal:
                    _gameDifficultySettings = (GameDifficultySettings)_settingsReader.Load(typeof(GameDifficultySettings), "GameDifficultyNormal");
                    break;
                case GameDifficulfy.Hard:
                    _gameDifficultySettings = (GameDifficultySettings)_settingsReader.Load(typeof(GameDifficultySettings), "GameDifficultyHard");
                    break;
            }
        }
    }
}