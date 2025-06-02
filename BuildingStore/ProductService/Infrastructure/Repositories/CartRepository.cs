using ProductService.Application.Interfaces;
using ProductService.Domain.DataBase;
using ProductService.Domain.Entities;

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
