using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CastleGo.Shared.Games;
using CastleGo.Shared.Settings;

namespace CastleGo.WebApi.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class TroopTypeProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public TroopTypeProfile()
        {
            CreateMap<AddTroopTypeModel, CastleTroopTypeModel>();
        }
    }
}