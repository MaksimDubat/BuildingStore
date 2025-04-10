using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;
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
        public async Task<CartItem> ChangeAmountAsync(int amount, int productId, int cartId, CancellationToken cancellation)
        {
            var product = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId, cancellation);   

            product.Amount = amount;
            
            return product;
        }

        /// <inheritdoc/>
        public async Task<CartItem> DeleteCartItemAsync(int cartId, int productId, CancellationToken cancellation)
        {
            var product = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId, cancellation);

            _context.Remove(product);

            return product;
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
