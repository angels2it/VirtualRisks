using CastleGo.Shared;
using CastleGo.Shared.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CastleGo.Application.Users
{
    public interface IUserService : IBaseService<UserDto>
    {
        Task<PagingResult<UserDto>> PagingAsync(PagingModel model);

        Task ActiveAsync(string id);

        Task DeactiveAsync(string id);

        Task DeleteAsync(string id);

        new Task InsertAsync(UserDto user);

        Task UpdateFriendsAsync(string id, List<FriendModel> friends);
        Task<PagingResult<FriendModel>> PagingFriendsAsync(PagingByIdModel pagingByIdModel);
        Task<bool> IsUserActivatedAsync(string userId);
        Task UpdateTokenAsync(string userId, UpdateTokenModel model);
        Task<UserDto> GetBySocialInfoAsync(OpponentExtInfoModel opponentExtInfo);
        Task UpdateHeroLocationAsync(string userId, UpdateHeroLocationModel model);
    }
}
