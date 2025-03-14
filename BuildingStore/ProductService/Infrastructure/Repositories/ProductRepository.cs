using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;

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
    }
}
