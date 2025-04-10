using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.DataBase;

namespace ProductService.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Реализация паттерна Unit of Work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MutableDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public ICartItemRepository CartItems { get; }
        public ICartRepository Carts { get; }

        public IOrderRepository Orders { get; }

        public UnitOfWork(MutableDbContext context, IServiceProvider serviceProvider, IProductRepository products, 
            ICategoryRepository categories, ICartItemRepository cartItems, ICartRepository carts, IOrderRepository orders)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            Products = products;
            Categories = categories;
            CartItems = cartItems;
            Carts = carts;
            Orders = orders;
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
    }
}
