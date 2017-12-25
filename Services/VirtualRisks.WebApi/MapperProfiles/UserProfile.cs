using AutoMapper;
using CastleGo.Application.Users;
using CastleGo.Providers.Models;
using CastleGo.Shared;
using CastleGo.Shared.Users;
using CastleGo.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace CastleGo.WebApi.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                      .ForMember(src => src.Id, opt => opt.ResolveUsing<ApplicationUserMapper>());
            CreateMap<UserLoginInfo, LoginsDto>();
            CreateMap<FacebookFriend, FriendModel>()
                .ForMember(des => des.Position,
                    opt => opt.MapFrom(src => (src.Location != null && src.Location.Location != null) ? new PositionModel()
                    {
                        Lat = src.Location.Location.Latitude,
                        Lng = src.Location.Location.Longitude,
                        Address = src.Location.Name
                    } : null));
        }
    }
}
