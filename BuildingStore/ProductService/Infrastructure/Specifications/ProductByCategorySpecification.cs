using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Specifications
{
    /// <summary>
    /// Получение продуктов по категории.
    /// </summary>
    public class ProductByCategorySpecification : BaseSpecification<Product>
    {
        public ProductByCategorySpecification(string CategoryName)
            : base(p => p.Category.CategoryName == CategoryName) { }
    }
}
