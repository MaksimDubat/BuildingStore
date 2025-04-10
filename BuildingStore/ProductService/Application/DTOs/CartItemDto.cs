namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности компонента корзины.
    /// </summary>
    public class CartItemDto
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Количество товара в корзине.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Продукт.
        /// </summary>
        public ProductDto? Product { get; set; }
    }
}
