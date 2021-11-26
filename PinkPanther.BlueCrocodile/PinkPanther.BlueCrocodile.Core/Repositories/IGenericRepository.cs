using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Core.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task<T> InsertAsync(T item);
        Task InsertManyAsync(List<T> orders);
        Task<T> UpdateAsync(T item);
        Task RemoveAllAsync();
        Task RemoveAsync(string id);
    }
}
