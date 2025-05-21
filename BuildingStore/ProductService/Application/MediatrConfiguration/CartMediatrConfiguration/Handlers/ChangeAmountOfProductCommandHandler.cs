using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для изменения количества товаров в корзине.
    /// </summary>
    public class ChangeAmountOfProductCommandHandler : IRequestHandler<ChangeAmountOfProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeAmountOfProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangeAmountOfProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.CartItems.GetCartItemAsync(request.CartId, request.ProductId, cancellationToken);

            if (item is null)
            {
                return Result.Failure("product in cart is empty");
            }

            if(request.Amount < 0)
            {
               return Result.Failure("amount should be more than 0");
            }

            item.Amount = request.Amount;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("changed");
        }
    }
}
