namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность категории.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Наименование категории.
        /// </summary>
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
