namespace ProductService.Domain.Entities
{
    /// <summary>
    /// Сущность отчета.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// Идентификатор продавца.
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Дата создания отчета.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Количество товаров.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public required string Description { get; set; }

        public Order Order { get; set; }
    }
}
