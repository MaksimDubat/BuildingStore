using Microsoft.EntityFrameworkCore;
using UserService.Domain.DataBase;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий по работе с пользователями.
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly MutableDbConext _context;

        public UserRepository(MutableDbConext context) : base(context)
        {
            _context = context; 
        }

        /// <inheritdoc/>
        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellation)
        {
            return await _context.User
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.UserEmail == email, cancellation);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetManagersAsync(CancellationToken cancellation)
        {
            return await _context.User
                .Where(x => x.Role == UserRole.Manager)
                .ToListAsync(cancellation);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUserExistOrDuplicateAsync(User user, CancellationToken cancellation)
        {
            return await _context.User.AnyAsync(
            x => x.Id != user.Id &&
                x.UserEmail == user.UserEmail.ToLower(),
                cancellation);
        }
    }
}
