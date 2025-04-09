using MediatR;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для применения промокода к товару.
    /// </summary>
    public class AddProductSaleCodeCommandHandler : IRequestHandler<AddProductSaleCodeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductSaleCodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddProductSaleCodeCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException("Not found");
            }

            if(request.Discount <= 0)
            {
                throw new ArgumentException("Wrong discount");
            }

            if(request.SaleEnd <= 0)
            {
                throw new ArgumentException("Wrong amount of days");
            }

            product.SaleCode = request.SaleCode;
            product.SalePrice = product.Price * (1 - (decimal)request.Discount / 100);
            product.SaleEndDate = DateTime.UtcNow.AddDays(request.SaleEnd);

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
