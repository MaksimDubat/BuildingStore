using MediatR;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Domain.Enums;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для изменения статуса заказа.
    /// </summary>
    public class ChangeStatusOfOrderCommandHandler : IRequestHandler<ChangeStatusOfOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusOfOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ChangeStatusOfOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new KeyNotFoundException("Cart not found");
            }

            order.Status = request.Status;
            
            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
