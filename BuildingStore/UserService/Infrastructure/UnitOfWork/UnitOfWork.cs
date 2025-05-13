using UserService.Application.Interfaces;
using UserService.Domain.DataBase;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Реализация паттерна Unit of Work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MutableDbConext _context;
        private readonly IServiceProvider _serviceProvider;
        public IUserRepository Users { get; }

        public UnitOfWork(MutableDbConext context, IServiceProvider serviceProvider, IUserRepository users)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            Users = users;
        }

        /// <inheritdoc/>
        public async Task<int> CompleteAsync(CancellationToken cancellation)
        {
            return await _context.SaveChangesAsync(cancellation);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <inheritdoc/>
        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            return ActivatorUtilities.CreateInstance<BaseRepository<T>>(_serviceProvider, _context);
        }
    }
}
