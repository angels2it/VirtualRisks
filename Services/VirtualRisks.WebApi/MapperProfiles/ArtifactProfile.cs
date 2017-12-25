using AutoMapper;
using CastleGo.Application.Artifacts.Dtos;

namespace CastleGo.WebApi.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ArtifactProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ArtifactProfile()
        {
            CreateMap<ArtifactModel, ArtifactOnMapModel>();
        }
    }
}