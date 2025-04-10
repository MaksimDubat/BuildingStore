using ProductService.Domain.Enums;

namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность заказа.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Статус заказа.
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// Итоговая сумма.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        public DateTime CreatedAt{ get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<Report> Reports { get; set; } = new List<Report>();

    }
}
