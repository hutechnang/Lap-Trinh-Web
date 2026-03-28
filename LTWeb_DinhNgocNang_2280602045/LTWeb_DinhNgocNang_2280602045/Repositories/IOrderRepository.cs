
using LTWeb_DinhNgocNang_2280602045.Models;

namespace LTWeb_DinhNgocNang_2280602045.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }

}