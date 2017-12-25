using System;
using System.Threading.Tasks;
using CastleGo.Application.Games;
using CastleGo.Shared.Common;
using Hangfire;

namespace CastleGo.GameAi
{
    /// <summary>
    /// 
    /// </summary>
    public class GameAiFactory : IGameAiFactory
    {
        private readonly IGameService _gameService;
        private readonly IAttackingGameAiService _attacking;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameService"></param>
        /// <param name="attacking"></param>
        public GameAiFactory(IGameService gameService, IAttackingGameAiService attacking)
        {
            _gameService = gameService;
            _attacking = attacking;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public async Task AnayticForGame(string id)
        {
            // start build game
            var game = await _gameService.Build(new Guid(id), string.Empty, -1);
            if (game == null || game.HasError)
                return;
            if (game.Status == GameStatus.Ended)
            {
                RecurringJob.RemoveIfExists(id);
                return;
            }
            // do something...
            await _attacking.Battalion(game);
        }
    }
}