using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс по работе с продуктами в корзине.
    /// </summary>
    public interface ICartItemRepository : IBaseRepository<CartItem>
    {
        /// <summary>
        /// Получение товаров из корзины.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cancellation"></param>
        Task<List<CartItem>> GetCartItemsAsync(int cartId, CancellationToken cancellation);

        /// <summary>
        /// Получение товара из корзины.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="cancellation"></param>
        Task<CartItem> GetCartItemAsync(int cartId, int productId, CancellationToken cancellation);
    }
}
