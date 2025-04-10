using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с заказами.
    /// </summary>
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly MutableDbContext _context;

        public OrderRepository(MutableDbContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellation)
        {
           return _context.Orders
                .Include(x => x.OrderItems)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public Task<Order> GetOrderByIdAsync(int id, CancellationToken cancellation)
        {
            return _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.OrderId == id, cancellation);
        }
    }
}
