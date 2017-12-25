using System;
using AutoMapper;
using MongoRepository;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CastleGo.Application
{
    public class BaseService<TDto, TEntity> : IBaseService<TDto> where TEntity : Entity
    {
        protected readonly IRepository<TEntity> Repository;

        protected BaseService(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        public async Task<TDto> GetByIdAsync(string id)
        {
            TEntity entity = await Repository.GetByIdAsync(id);
            return Mapper.Map<TDto>(entity);
        }

        public Task UpdateAsync(TDto model)
        {
            return Repository.UpdateAsync(Mapper.Map<TEntity>((object)model));
        }
        public Task InsertAsync(TDto model)
        {
            var entity = Mapper.Map<TEntity>(model);
            if (string.IsNullOrEmpty(entity.Id))
                entity.Id = Guid.NewGuid().ToString();
            return Repository.AddAsync(entity);
        }

        public Task RemoveAsync(string id)
        {
            return Repository.Collection.DeleteOneAsync(entity => entity.Id == id);
        }
    }
}
