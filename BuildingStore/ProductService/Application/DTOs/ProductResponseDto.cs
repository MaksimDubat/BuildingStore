namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO ответа для сущности продукт.
    /// </summary>
    public class ProductResponseDto
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
        public string Image { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Цена со скидкой.
        /// </summary>
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// Дата окончания скидки.
        /// </summary>
        public DateTime? SaleEndDate { get; set; }
    }
}
