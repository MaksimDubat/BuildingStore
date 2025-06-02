
using ProductService.Application.Interfaces;

namespace ProductService.Application.Services
{
    /// <summary>
    /// Сервис проверки действительности скидок.
    /// </summary>
    public class SaleCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public SaleCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanExpiredSales(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task CleanExpiredSales(CancellationToken cancellation)
        {
            using var scope = _scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var exipredProducts = await unitOfWork.Products.GetExpiredSalesAsync(cancellation);

            foreach ( var product in exipredProducts)
            {
                product.SaleCode = null;
                product.SalePrice = null;
                product.SaleEndDate = null;

                await unitOfWork.Products.UpdateAsync(product, cancellation);
            }

            await unitOfWork.CompleteAsync(cancellation);
        }
    }
}
