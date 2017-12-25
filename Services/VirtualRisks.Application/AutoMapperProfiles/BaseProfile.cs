using AutoMapper;
using CastleGo.Application.Clients;
using CastleGo.Entities;
using CastleGo.Shared;
using MongoRepository;

namespace CastleGo.Application.AutoMapperProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<PagingModel, DataAccess.Models.PagingModel>();
            CreateMap<PagingByIdModel, DataAccess.Models.PagingByIdModel>();
            CreateMap<Entity, BaseDto>();
            CreateMap<BaseDto, Entity>();
            CreateMap<PositionModel, Position>();
            CreateMap<Position, PositionModel>();

            CreateMap<Client, ClientDto>();
        }
    }
}
