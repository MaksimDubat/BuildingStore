namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность компонентов заказа.
    /// </summary>
    public class OrderItem
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
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
