namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность компонентов корзины.
    /// </summary>
    public class CartItem
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
        /// Количество.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Корзина.
        /// </summary>
        public Cart Cart { get; set; }
        /// <summary>
        /// Продукты.
        /// </summary>
        public Product Product { get; set; }

    }
}
