using AutoMapper;
using CastleGo.Domain.Aggregates;

namespace CastleGo.Domain.Service
{
    public class SoldierProfile : Profile
    {
        public SoldierProfile()
        {
            CreateMap<SoldierAggregate, SoldierAggregate>();
        }
    }
}
