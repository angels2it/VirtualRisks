using CastleGo.DataAccess.Models;
using CastleGo.Entities;
using CastleGo.Shared.Common;
using MongoDB.Driver;
using MongoRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CastleGo.DataAccess
{
    public static class GameRepository
    {
        public static Task AcceptedAsync(this IRepository<Game> repository, string id, string userId, string heroId)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id, Builders<Game>.Update.Set(e => e.Status, GameStatus.Playing).Set(e => e.OpponentId, userId).Set(e => e.OpponentHeroId, heroId));
        }

        public static Task SetOpponentArmySettingAsync(this IRepository<Game> repository, string id,
            GameArmySetting opponentArmy)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<Game>.Update
                .Set(e => e.OpponentArmySetting, opponentArmy));
        }
        public static Task AcceptedSelfPlayingAsync(this IRepository<Game> repository, string id)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<Game>.Update.Set(e => e.Status, GameStatus.Playing));
        }

        public static Task UpdateStateAsync(this IRepository<Game> repository, string id, UpdateGameStateDataModel state)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id, Builders<Game>.Update.Set(e => e.Status, state.Status).Set(e => e.RedCastleAmount, state.RedCastleAmount).Set(e => e.BlueCastleAmount, state.BlueCastleAmount).Set(e => e.NeutrualCastleAmount, state.NeutrualCastleAmount));
        }
        public static Task RemoveAsync(this IRepository<Game> repository, string id)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<Game>.Update.Set(e => e.IsRemoved, true));
        }
        public static Task GenerateCastlesAsync(this IRepository<Game> repository, string id, List<string> castles)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id, Builders<Game>.Update.Set(e => e.Castles, castles));
        }
        public static Task UpdateStreamVersionAsync(this IRepository<Game> repository, string id, int streamVersion, bool isOpponent)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id,
                isOpponent ? Builders<Game>.Update.Set(e => e.OpponentStreamVersion, streamVersion) :
                Builders<Game>.Update.Set(e => e.UserStreamVersion, streamVersion));
        }
        public static async Task<PagingResult<Game>> PagingAsync(this IRepository<Game> repository, PagingByIdModel model)
        {
            var userCollection = repository.Collection.Database.GetCollection<User>("User");
            var user = await userCollection.Find(Builders<User>.Filter.Eq(e => e.Id, model.Id)).FirstOrDefaultAsync();
            if (user == null)
                return null;
            var keys = user.Logins?.Select(e => e.ProviderKey);
            var availableFilter = Builders<Game>.Filter.Where(e => e.IsRemoved != true);
            var statusFilter = Builders<Game>.Filter.Where(e => e.Status != GameStatus.Ended || e.Status != GameStatus.Rejected);
            var belongsToUserFilter =
                Builders<Game>.Filter.Where(e => e.CreatedBy == model.Id || e.OpponentId == model.Id);
            var socialInviteFilter = Builders<Game>.Filter.And(Builders<Game>.Filter.Where(e => e.OpponentExtInfo != null),
                Builders<Game>.Filter.In(e => e.OpponentExtInfo.Key, keys));
            var queryable = repository.Collection.Find(Builders<Game>.Filter.And(availableFilter, statusFilter, Builders<Game>.Filter.Or(belongsToUserFilter, socialInviteFilter)));
            var pagingResult = new PagingResult<Game>
            {
                Total = await queryable.CountAsync(),
                Items = await queryable.Sort(Builders<Game>.Sort.Descending(e => e.CreatedAt)).Skip(model.Skip).Limit(model.Take).ToListAsync() ?? new List<Game>()
            };
            pagingResult.CanLoadMore = pagingResult.Total > model.Skip + model.Take;
            return pagingResult;
        }

        public static async Task<List<Game>> GetAllPlayingGameOfHero(this IRepository<Game> repository, string userId, string heroId)
        {
            var filter = Builders<Game>.Filter.Where(e =>
            e.Status == GameStatus.Playing &&
            (e.CreatedBy == userId && e.UserHeroId == heroId) || (e.OpponentId == userId && e.OpponentHeroId == heroId));
            return await repository.Collection.Find(filter).ToListAsync() ?? new List<Game>();
        }
        public static async Task<List<Game>> GetAllPlayingGame(this IRepository<Game> repository)
        {
            var filter = Builders<Game>.Filter.Where(e => e.Status == GameStatus.Playing);
            return await repository.Collection.Find(filter).ToListAsync() ?? new List<Game>();
        }
    }
}
