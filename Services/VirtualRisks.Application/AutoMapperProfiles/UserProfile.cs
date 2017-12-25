using AutoMapper;
using CastleGo.Application.Users;
using CastleGo.Entities;
using CastleGo.Shared;
using CastleGo.Shared.Users;

namespace CastleGo.Application.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserModel>();
            CreateMap<UserDto, User>();
            CreateMap<Logins, LoginsDto>();
            CreateMap<LoginsDto, Logins>();
            CreateMap<NotifyToken, NotifyTokenDto>();
            CreateMap<NotifyTokenDto, NotifyToken>();

            CreateMap<Hero, HeroModel>();
            CreateMap<HeroModel, Hero>();
            CreateMap<FriendModel, Friend>();
            CreateMap<Friend, FriendModel>();
            CreateMap<OpponentExtInfoModel, OpponentExtInfo>();
            CreateMap<OpponentExtInfo, OpponentExtInfoModel>();
        }
    }
}
