using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Specifications
{
    /// <summary>
    /// Фильтрация продукта по цене.
    /// </summary>
    public class ProductByPriceSpecification : BaseSpecification<Product>
    {
        public ProductByPriceSpecification(decimal minPrice, decimal maxPrice, bool orderByAscending = true)
            : base(p => p.Price >= minPrice && p.Price <= maxPrice) { }
    }
}
