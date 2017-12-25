using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CastleGo.Application.Settings.Dtos;
using CastleGo.Entities;
using MongoDB.Driver;
using MongoRepository;

namespace CastleGo.Application.Settings
{
    public interface IGameArmySettingService : IBaseService<GameArmySettingModel>
    {
        Task<List<GameArmySettingModel>> GetAllAsync();
        Task<bool> IsArmyExistingAsync(string name);

        Task<GameArmySettingModel> GetByNameAsync(string name);
    }

    public class GameArmySettingService : BaseService<GameArmySettingModel, GameArmySetting>, IGameArmySettingService
    {
        public GameArmySettingService(IRepository<GameArmySetting> repository) : base(repository)
        {
        }

        public async Task<List<GameArmySettingModel>> GetAllAsync()
        {
            var items = await Repository.Collection.Find(FilterDefinition<GameArmySetting>.Empty).ToListAsync() ??
                        new List<GameArmySetting>();
            return Mapper.Map<List<GameArmySettingModel>>(items);
        }

        public Task<bool> IsArmyExistingAsync(string name)
        {
            return Repository.Collection.Find(setting => setting.Name == name).AnyAsync();
        }

        
        public async Task<GameArmySettingModel> GetByNameAsync(string name)
        {
            var army =await  Repository.Collection.Find(setting => setting.Name == name).FirstOrDefaultAsync();
            if (army == null)
                return null;
            return Mapper.Map<GameArmySettingModel>(army);
        }
    }
}
