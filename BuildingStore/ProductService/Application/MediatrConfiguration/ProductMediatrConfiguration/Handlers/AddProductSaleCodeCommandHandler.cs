using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для применения промокода к товару.
    /// </summary>
    public class AddProductSaleCodeCommandHandler : IRequestHandler<AddProductSaleCodeCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductSaleCodeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddProductSaleCodeCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.ProductId, cancellationToken);

            if (product is null)
            {
                return Result.Failure("Not found");
            }

            if(request.Discount <= 0)
            {
                return Result.Failure("Wrong discount");
            }

            if(request.SaleEnd <= 0)
            {
                return Result.Failure("Wrong amount of days");
            }

            product.SaleCode = request.SaleCode;
            product.SalePrice = product.Price * (1 - (decimal)request.Discount / 100);
            product.SaleEndDate = DateTime.UtcNow.AddDays(request.SaleEnd);

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Added");
        }
    }
}
