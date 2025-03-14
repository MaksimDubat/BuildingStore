namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность продукт.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Описание продукта.
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// Цена продукта.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Идентифкатор категории.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// URL изображения.
        /// </summary>
        public string ImageURL { get; set; }
        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Дата обновления.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        public Category Category { get; set; }
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
