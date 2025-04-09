using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;
using System.Threading;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с категориями.
    /// </summary>
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly MutableDbContext _context;

        public CategoryRepository(MutableDbContext context) : base(context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<bool> IsCategoryExistOrDuplicateAsync(Category category, CancellationToken cancellation)
        {
            return await _context.Categories.AnyAsync(
                x => x.CategoryId != category.CategoryId &&
                x.CategoryName == category.CategoryName, 
                cancellation);
        }
    }
}
