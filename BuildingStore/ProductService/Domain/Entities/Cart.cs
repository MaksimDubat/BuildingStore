namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность корзины.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Количество товаров.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Продукт.
        /// </summary>
        public Product Product { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
