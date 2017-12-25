using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CastleGo.Entities;
using CastleGo.Shared.Games;
using CastleGo.Shared.Settings;
using MongoDB.Driver;
using MongoRepository;

namespace CastleGo.Application.CastleTroopTypes
{
    public interface ICastleTroopTypeService : IBaseService<CastleTroopTypeModel>
    {
        Task<List<CastleTroopTypeModel>> GetAllAsync();
        Task<CastleTroopTypeModel> GetByTypeAsync(string resourceType);
    }

    public class CastleTroopTypeService : BaseService<CastleTroopTypeModel, CastleTroopType>, ICastleTroopTypeService
    {
        public CastleTroopTypeService(IRepository<CastleTroopType> repository) : base(repository)
        {
        }

        public async Task<List<CastleTroopTypeModel>> GetAllAsync()
        {
            var troopTypes = await Repository.Collection.Find(FilterDefinition<CastleTroopType>.Empty).ToListAsync() ?? new List<CastleTroopType>();
            return Mapper.Map<List<CastleTroopTypeModel>>(troopTypes);
        }

        public async Task<CastleTroopTypeModel> GetByTypeAsync(string resourceType)
        {
            var troopType =
                await Repository.Collection.Find(Builders<CastleTroopType>.Filter.Eq(e => e.ResourceType, resourceType))
                    .FirstOrDefaultAsync();
            if (troopType == null)
                return null;
            return Mapper.Map<CastleTroopTypeModel>(troopType);
        }
    }
}
