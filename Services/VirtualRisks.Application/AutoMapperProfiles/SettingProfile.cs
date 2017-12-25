using AutoMapper;
using CastleGo.Application.Artifacts.Dtos;
using CastleGo.Application.Settings.Dtos;
using CastleGo.Entities;

namespace CastleGo.Application.AutoMapperProfiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<GameArmySetting, GameArmySettingModel>();
            CreateMap<GameArmySettingModel, GameArmySetting>();

            CreateMap<GameCastleSetting, GameCastleSettingModel>();
            CreateMap<GameCastleSettingModel, GameCastleSetting>();


            CreateMap<Artifact, ArtifactModel>();
            CreateMap<ArtifactModel, Artifact>();

            CreateMap<ArtifactPrize, ArtifactPrizeModel>();
            CreateMap<ArtifactPrizeModel, ArtifactPrize>();
        }
    }
}
