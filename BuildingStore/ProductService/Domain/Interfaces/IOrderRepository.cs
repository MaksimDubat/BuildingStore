using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория по работе с заказами.
    /// </summary>
    public interface IOrderRepository : IBaseRepository<Order>
    {
        /// <summary>
        /// Получение всех заказов.
        /// </summary>
        /// <param name="cancellation"></param>
        Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellation);

        /// <summary>
        /// Получение заказа по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        Task<Order> GetOrderByIdAsync(int id, CancellationToken cancellation);
    }
}
