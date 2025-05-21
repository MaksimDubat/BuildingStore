using ProductService.Domain.Entities;

namespace ProductService.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория по работе с заказами.
    /// </summary>
    public interface IOrderRepository : IBaseRepository<Order>
    {
        /// <summary>
        /// Получение заказа по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        Task<Order> GetOrderByIdAsync(int id, CancellationToken cancellation);
    }
}
