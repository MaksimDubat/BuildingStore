using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды добавления товара в корзину.
    /// </summary>
    public class AddProductToCartCommand : IRequest
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Наименование товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public int Amount { get; set; } 

        public AddProductToCartCommand(int cartId, int productId, int amount)
        {
            CartId = cartId;
            ProductId = productId;
            Amount = amount;
        }
    }
}
