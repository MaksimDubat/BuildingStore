using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления продукта из корзины.
    /// </summary>
    public class DeleteProductFromCartCommandHandler : IRequestHandler<DeleteProductFromCartCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteProductFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.GetCartItemAsync(request.CartId, request.ProductId, cancellationToken);
               
            if (cartItem is null)
            {
                return Result.Failure($"Product {request.ProductId} not found in cart {request.CartId}.");
            }

            await _unitOfWork.CartItems.DeleteEntityAsync(cartItem, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Deleted");

        }
    }
}
