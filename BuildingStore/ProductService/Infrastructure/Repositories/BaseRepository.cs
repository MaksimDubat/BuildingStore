using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;
using System.Linq.Expressions;

namespace ProductService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с CRUD-операциями.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly MutableDbContext _context;

        public BaseRepository(MutableDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task AddAsync(T entity, CancellationToken cancellation)
        {
            await _context.AddAsync(entity, cancellation);
        }

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .AnyAsync(predicate, cancellation);
        }

        /// <inheritdoc/>
        public async Task<T> DeleteAsync(int id, CancellationToken cancellation)
        {
            var entity = await _context.Set<T>().FindAsync(id, cancellation);
            _context.Remove(entity);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<T> DeleteEntityAsync(T entity, CancellationToken cancellation)
        {
            _context.Remove(entity);
            return entity;
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAllAsync(CancellationToken cancellation)
        {
            var result = await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellation);

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync(int id, CancellationToken cancellation)
        {
            return await _context.Set<T>().FindAsync(id, cancellation);
        }

        // <inheritdoc/>
        public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellation)
        {
            IQueryable<T> query = _context.Set<T>()
                .Where(specification.Criteria);

            return await query.AsNoTracking().ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
             _context.Update(entity);
        }
    }
}
