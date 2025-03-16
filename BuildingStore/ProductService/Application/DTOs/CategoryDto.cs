namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности категории.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Наименование категории.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
