using MediatR;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для применения промокода к товару.
    /// </summary>
    public class AddProductSaleCodeCommand : IRequest
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Промокод.
        /// </summary>
        public string? SaleCode { get; set; }

        /// <summary>
        /// Процент скидки.
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// Конец скидки
        /// </summary>
        public int SaleEnd { get; set; }

        public AddProductSaleCodeCommand(int productId, string saleCode, int discount, int saleEnd)
        {
            ProductId = productId;
            SaleCode = saleCode;
            Discount = discount;
            SaleEnd = saleEnd;
        }
    }
}
