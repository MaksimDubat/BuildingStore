using ProductService.Domain.Entities;

namespace ProductService.Application.DTOs
{
    /// <summary>
    /// DTO для сущности корзины.
    /// </summary>
    public class CartDto
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
        /// Список товаров в корзине.
        /// </summary>
        public List<CartItemDto>? Items { get; set; } = new();
    }
}
