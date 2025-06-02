using MediatR;
using ProductService.Application.Common;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель запроса на заказ всех продуктов из корзины. 
    /// </summary>
    public class PurchaseAllProductsCommand : IRequest<Result>
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }

        public PurchaseAllProductsCommand(int cartId)
        {
            CartId = cartId;
        }
    }
}
