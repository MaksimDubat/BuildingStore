using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Specifications
{
    /// <summary>
    /// Получение продуктов по наличию.
    /// </summary>
    public class ProductByAmountSpecification : BaseSpecification<Product>
    {
        public ProductByAmountSpecification()
            : base(p => p.Amount > 0) { }
    }
}
