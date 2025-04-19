using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;

namespace ProductService.Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly MutableDbContext _context;

        public CartRepository(MutableDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
