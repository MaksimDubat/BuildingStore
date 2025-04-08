using MediatR;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для заказа одного товара.
    /// </summary>
    public class PurchaseSingleProductCommand : IRequest
    {
        /// <summary>
        /// Идентификатор корзины.
        /// </summary>
        public int CartId { get; set; }   

        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Промокод.
        /// </summary>
        public string? SaleCode { get; set; }

        public PurchaseSingleProductCommand(int cartId, int productId, string saleCode)
        {
            CartId = cartId;
            ProductId = productId;
            SaleCode = saleCode;
        }
    }
}
