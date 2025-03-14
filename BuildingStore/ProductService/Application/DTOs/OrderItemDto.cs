namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности компонента заказа.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// Идентификатор описания заказа.
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Количество товаров.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Итоговая сумма заказа.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
