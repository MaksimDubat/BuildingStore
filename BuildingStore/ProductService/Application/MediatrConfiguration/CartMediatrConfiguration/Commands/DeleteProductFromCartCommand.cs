using MediatR;
using ProductService.Application.Common;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для удаления продукта из корзины.
    /// </summary>
    public class DeleteProductFromCartCommand : IRequest<Result>
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }

        public DeleteProductFromCartCommand(int cartId, int productId)
        {
            CartId = cartId;
            ProductId = productId;
        }
    }
}
