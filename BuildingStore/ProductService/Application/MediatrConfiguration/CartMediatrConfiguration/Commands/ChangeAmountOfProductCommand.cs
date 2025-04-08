using MediatR;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для изменения количества товаров в корзине.
    /// </summary>
    public class ChangeAmountOfProductCommand : IRequest
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
        /// Количество.
        /// </summary>
        public int Amount {  get; set; }

        public ChangeAmountOfProductCommand(int cartId, int productId, int amount)
        {
            CartId = cartId;
            ProductId = productId;
            Amount = amount;
        }
    }
}
