
using LTWeb_DinhNgocNang_2280602045.Models;
using Microsoft.EntityFrameworkCore;

namespace LTWeb_DinhNgocNang_2280602045.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public EFOrderRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ToListAsync();

        public async Task<Order> GetByIdAsync(int id)
            => await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }

}