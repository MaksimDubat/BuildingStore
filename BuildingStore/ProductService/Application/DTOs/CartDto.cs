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
    }
}
