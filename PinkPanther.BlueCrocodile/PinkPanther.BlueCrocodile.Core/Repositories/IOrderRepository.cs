using PinkPanther.BlueCrocodile.Core.Models;
using System.Threading.Tasks;

namespace PinkPanther.BlueCrocodile.Core.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetByCodeAsync(string code);
    }
}
