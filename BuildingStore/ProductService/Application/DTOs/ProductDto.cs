namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности продукт.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Описание продукта.
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Категория.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Цена продукта.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Изображение.
        /// </summary>
        public string ImageURL { get; set; }
        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

    }
}
