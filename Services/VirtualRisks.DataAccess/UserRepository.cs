using CastleGo.DataAccess.Models;
using CastleGo.Entities;
using MongoDB.Driver;
using MongoRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CastleGo.Shared.Common;

namespace CastleGo.DataAccess
{
    public static class UserRepository
    {
        public static async Task<PagingResult<User>> PagingAsync(this IRepository<User> repository, PagingModel model)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Ne(e => e.IsDelete, 1 != 0);
            var searchFilter = !string.IsNullOrEmpty(model.SearchTextFormated) ? Builders<User>.Filter.Where(e => e.Name.ToLower().Contains(model.SearchTextFormated) || e.Email.ToLower().Contains(model.SearchTextFormated)) : Builders<User>.Filter.Empty;

            var queryable = repository.Collection.Find(Builders<User>.Filter.And(filter, searchFilter));
            var total = await queryable.CountAsync();

            var result = new PagingResult<User>
            {
                Items = await queryable.Skip(model.Skip).Limit(model.Take).ToListAsync() ?? new List<User>(),
                CanLoadMore = total > model.Skip + model.Take,
            };
            return result;
        }

        public static Task ChangeStatusAsync(this IRepository<User> repository, string id, bool active)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id, Builders<User>.Update.Set(e => e.IsActive, (active ? 1 : 0) != 0));
        }
        public static Task UpdateHeroLocationAsync(this IRepository<User> repository, string id, string heroId, Position position)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id && e.Heroes.Any(f => f.Id == heroId),
                Builders<User>.Update.Set(e => e.Heroes[-1].Position, position));
        }

        public static Task UpdateFriendsAsync(this IRepository<User> repository, string id, List<Friend> friends)
        {
            return repository.Collection.UpdateOneAsync(e => e.Id == id, Builders<User>.Update.Set(e => e.Friends, friends));
        }
        public static async Task UpdateTokenAsync(this IRepository<User> repository, string id, NotifyToken token)
        {
            var user = await repository.Collection.Find(Builders<User>.Filter.Eq(e => e.Id, id)).FirstOrDefaultAsync();
            if (user == null)
                return;
            if (user.Tokens == null)
            {
                await repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<User>.Update.Set(e => e.Tokens, new List<NotifyToken>()
                {
                    token
                }));
                return;
            }
            await repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<User>.Update.PullFilter(e => e.Tokens, notifyToken => notifyToken.Device == MobileDevice.Android));
            await repository.Collection.UpdateOneAsync(e => e.Id == id,
                Builders<User>.Update.Push(e => e.Tokens, token));
        }
        public static async Task<PagingResult<Friend>> PagingFriendsAsync(this IRepository<User> repository, PagingByIdModel model)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(e => e.Id == model.Id && !e.IsDelete);
            var searchFilter = !string.IsNullOrEmpty(model.SearchTextFormated) ?
                Builders<UnwindUserFriend>.Filter.Where(e => e.Friends.Name.ToLower().Contains(model.SearchTextFormated)) :
                Builders<UnwindUserFriend>.Filter.Empty;

            var queryable = repository.Collection.Aggregate()
                .Match(filter)
                .Unwind<User, UnwindUserFriend>(e => e.Friends)
                .Match(searchFilter);
            var total = (await queryable.Group<UnwindUserFriend, string, TotalResult>(e => null, e => new TotalResult()
            {
                Result = e.Count()
            }).FirstOrDefaultAsync())?.Result ?? 0;
            var result = new PagingResult<Friend>
            {
                Items = (await queryable.Skip(model.Skip).Limit(model.Take).ToListAsync())?.Select(e => e.Friends).ToList() ?? new List<Friend>(),
                CanLoadMore = total > model.Skip + model.Take,
                Total = total
            };
            return result;
        }

        public static Task<User> GetBySocialInfoAsync(this IRepository<User> repository, string provider,
            string key)
        {
            var filter = Builders<User>.Filter.And(Builders<User>.Filter.Where(e => e.Logins != null),
                Builders<User>.Filter.ElemMatch(e => e.Logins, l => l.LoginProvider == provider && l.ProviderKey == key));
            return repository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        public static Hero GetHeroById(this IRepository<User> repository, string id,
            string heroId)
        {
            var user = repository.Collection.Find(Builders<User>.Filter.Eq(e => e.Id, id)).FirstOrDefault();
            return user?.Heroes?.FirstOrDefault(e => e.Id == heroId);
        }
        public static async Task<Hero> GetHeroByIdAsync(this IRepository<User> repository, string id,
            string heroId)
        {
            var user = await repository.Collection.Find(Builders<User>.Filter.Eq(e => e.Id, id)).FirstOrDefaultAsync();
            return user?.Heroes?.FirstOrDefault(e => e.Id == heroId);
        }
    }
}
