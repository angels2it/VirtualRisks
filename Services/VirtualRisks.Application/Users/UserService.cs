using System;
using AutoMapper;
using CastleGo.Entities;
using CastleGo.Shared;
using CastleGo.Shared.Users;
using MongoRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using CastleGo.DataAccess;

namespace CastleGo.Application.Users
{
    public class UserService : BaseService<UserDto, User>, IUserService
    {
        public UserService(IRepository<User> repository)
          : base(repository)
        {
        }

        public async Task<Shared.PagingResult<UserDto>> PagingAsync(Shared.PagingModel model)
        {
            var pagingResult = await Repository.PagingAsync(Mapper.Map<DataAccess.Models.PagingModel>(model));
            Shared.PagingResult<UserDto> result = new Shared.PagingResult<UserDto> { Total = pagingResult.Total };
            int num = pagingResult.CanLoadMore ? 1 : 0;
            result.CanLoadMore = num != 0;
            List<UserDto> userDtoList = Mapper.Map<List<UserDto>>(pagingResult.Items);
            result.Items = userDtoList;
            return result;
        }

        public Task ActiveAsync(string id)
        {
            return Repository.ChangeStatusAsync(id, true);
        }

        public Task DeactiveAsync(string id)
        {
            return Repository.ChangeStatusAsync(id, false);
        }

        public async Task DeleteAsync(string id)
        {
            User user1 = await Repository.GetByIdAsync(id);
            user1.IsDelete = true;
            await Repository.UpdateAsync(user1);
        }

        public new Task InsertAsync(UserDto user)
        {
            var entity = Mapper.Map<User>(user);
            entity.Heroes = new List<Hero>()
            {
                new Hero()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = user.Name,
                    Leadership = 2
                }
            };
            return Repository.AddAsync(entity);
        }

        public Task UpdateFriendsAsync(string id, List<FriendModel> friends)
        {
            return Repository.UpdateFriendsAsync(id, Mapper.Map<List<Friend>>(friends));
        }

        public async Task<Shared.PagingResult<FriendModel>> PagingFriendsAsync(Shared.PagingByIdModel pagingByIdModel)
        {
            var paging = await Repository.PagingFriendsAsync(Mapper.Map<DataAccess.Models.PagingByIdModel>(pagingByIdModel));
            var result = new Shared.PagingResult<FriendModel>();
            if (paging?.Items == null)
                return result;
            result.Items = Mapper.Map<List<FriendModel>>(paging.Items);
            result.Total = paging.Total;
            result.CanLoadMore = paging.CanLoadMore;
            return result;
        }

        public async Task<bool> IsUserActivatedAsync(string userId)
        {
            var user = await Repository.GetByIdAsync(userId);
            return user != null && user.IsActive && !user.IsDelete;
        }

        public Task UpdateTokenAsync(string userId, UpdateTokenModel model)
        {
            return Repository.UpdateTokenAsync(userId, new NotifyToken()
            {
                Device = model.Device,
                Token = model.Token
            });
        }

        public async Task<UserDto> GetBySocialInfoAsync(OpponentExtInfoModel opponentExtInfo)
        {
            var user = await Repository.GetBySocialInfoAsync(opponentExtInfo.Provider, opponentExtInfo.Key);
            return Mapper.Map<UserDto>(user);
        }

        public Task UpdateHeroLocationAsync(string userId, UpdateHeroLocationModel model)
        {
            model.Position.UpdatedAt = DateTime.UtcNow;
            return Repository.UpdateHeroLocationAsync(userId, model.HeroId, Mapper.Map<Position>(model.Position));
        }
    }
}
