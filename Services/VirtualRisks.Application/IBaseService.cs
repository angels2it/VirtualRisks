using System.Threading.Tasks;

namespace CastleGo.Application
{
    public interface IBaseService<TDto>
    {
        Task<TDto> GetByIdAsync(string id);
        Task UpdateAsync(TDto model);
        Task InsertAsync(TDto model);
        Task RemoveAsync(string id);
    }
}
