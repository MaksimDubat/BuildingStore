using MediatR;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для изменения количества товаров в корзине.
    /// </summary>
    public class ChangeAmountOfProductCommandHandler : IRequestHandler<ChangeAmountOfProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeAmountOfProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ChangeAmountOfProductCommand request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.CartItems.ChangeAmountAsync(request.Amount, request.ProductId, request.CartId, cancellationToken);

            if (item == null)
            {
                throw new KeyNotFoundException("product in cart is empty");
            }

            if(request.Amount < 0)
            {
                throw new ArgumentException("amount should be more than 0");
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
