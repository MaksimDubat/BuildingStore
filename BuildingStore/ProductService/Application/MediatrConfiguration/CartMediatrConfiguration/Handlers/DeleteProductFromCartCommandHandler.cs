using MediatR;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления продукта из корзины.
    /// </summary>
    public class DeleteProductFromCartCommandHandler : IRequestHandler<DeleteProductFromCartCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartItem = await _unitOfWork.CartItems.
                DeleteCartItemAsync(request.CartId, request.ProductId, cancellationToken);

            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Product {request.ProductId} not found in cart {request.CartId}.");
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
