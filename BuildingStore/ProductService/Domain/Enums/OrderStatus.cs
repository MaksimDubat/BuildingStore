namespace ProductService.Domain.Enums
{
    /// <summary>
    /// Статусы заказа.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// В ожидании.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Отправлен.
        /// </summary>
        Shipped = 2,

        /// <summary>
        /// Завершен.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Отменен.
        /// </summary>
        Cancelled = 4,
    }
}
