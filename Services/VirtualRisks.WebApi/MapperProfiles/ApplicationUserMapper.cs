using AutoMapper;
using CastleGo.Application.Users;
using CastleGo.WebApi.Models;
using MongoDB.Bson;

namespace CastleGo.WebApi.MapperProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserMapper : IValueResolver<ApplicationUser, UserDto, string>
    {
        /// <inheritdoc />
        public string Resolve(ApplicationUser source, UserDto destination, string destMember, ResolutionContext context)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(source.Id, out objectId))
                return source.Id;
            return objectId.AsGuid().ToString();
        }
    }
}
