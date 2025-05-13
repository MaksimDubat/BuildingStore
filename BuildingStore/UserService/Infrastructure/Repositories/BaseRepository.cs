using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Application.Extensions;
using UserService.Application.Interfaces;
using UserService.Domain.DataBase;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с CRUD-операциями.
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly MutableDbConext _context;

        public BaseRepository(MutableDbConext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task AddEntityAsync(T entity, CancellationToken cancellation)
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
        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder, CancellationToken cancellation)
        {
            var query = queryBuilder(_context.Set<T>());
            return await query.ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync(int id, CancellationToken cancellation)
        {
            return await _context.Set<T>().FindAsync(id, cancellation);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            _context.Update(entity);
        }
    }
}
