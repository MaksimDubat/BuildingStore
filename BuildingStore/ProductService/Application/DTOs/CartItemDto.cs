namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности компонента корзины.
    /// </summary>
    public class CartItemDto
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара в корзине.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Цена продукта.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Описание продукта.
        /// </summary>
        public string Description { get; set; }
    }
}
