namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности заказа.
    /// </summary>
    public class OrderDto
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
        public string Status { get; set; }

        /// <summary>
        /// Итоговая сумма.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Дата создания заказа.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Список товаров в заказе.
        /// </summary>
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        /// <summary>
        /// Список отчетов, связанных с заказом.
        /// </summary>
        public List<ReportDto> Reports { get; set; } = new List<ReportDto>();
    }
}
