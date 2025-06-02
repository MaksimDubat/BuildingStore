using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.DataBase;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;

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
        public Task<Order> GetOrderByIdAsync(int id, CancellationToken cancellation)
        {
            return _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.OrderId == id, cancellation);
        }
    }
}
