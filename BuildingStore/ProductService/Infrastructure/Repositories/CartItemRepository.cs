using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.DataBase;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.UnitOfWork;
using System.Threading;

namespace ProductService.Infrastructure.Repositories
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly MutableDbContext _context;

        public CartItemRepository(MutableDbContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<CartItem>> GetCartItemsAsync(int cartId, CancellationToken cancellation)
        {
            return await _context.CartItems
                .Where(x => x.CartId == cartId)
                .Include(x => x.Product)
                .ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<CartItem> GetCartItemAsync(int cartId, int productId, CancellationToken cancellation)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId, cancellation);
        }
    }
}
