using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Specifications
{
    /// <summary>
    /// Получение товаров по новизне.
    /// </summary>
    public class ProductByDateSpecification : BaseSpecification<Product>
    {
        public ProductByDateSpecification(int daysAgo, bool orderByAscending = true)
            : base(p => p.CreatedAt >= DateTime.UtcNow.AddDays(-daysAgo)) { }
    }
}
