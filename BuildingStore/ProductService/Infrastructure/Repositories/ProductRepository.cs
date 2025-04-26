using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;
using System.Threading;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с продуктом.
    /// </summary>
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly MutableDbContext _context;

        public ProductRepository(MutableDbContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Product> GetByNameAsync(string name, CancellationToken cancellation)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name, cancellation);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetExpiredSalesAsync(CancellationToken cancellation)
        {
            return await _context.Products
                .Where(x => x.SaleEndDate.HasValue && x.SaleEndDate < DateTime.UtcNow)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductsWithoutSale(CancellationToken cancellation)
        {
            return await _context.Products
                .Where(x => x.SalePrice == null && x.SaleEndDate == null && x.SaleCode == null)
                .ToListAsync(cancellation);
        }

        // <inheritdoc/>
        public async Task<IEnumerable<Product>> GetProductsWithSale(CancellationToken cancellation)
        {
            return await _context.Products
                .Where(x => string.IsNullOrEmpty(x.SaleCode) && x.SalePrice != null)
                .ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<bool> IsProductExistOrDuplicateAsync(Product product, CancellationToken cancellation)
        {
            return await _context.Products.AnyAsync(
            x => x.ProductId != product.ProductId &&
                x.Name == product.Name.ToLower() &&
                x.Description == product.Description.ToLower() &&
                x.Price == product.Price &&
                x.Amount == product.Amount,
                cancellation);

        }
    }
}
