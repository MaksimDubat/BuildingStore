using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Commands;
using ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands;
using ProductService.Domain.Enums;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для изменения статуса заказа.
    /// </summary>
    public class ChangeStatusOfOrderCommandHandler : IRequestHandler<ChangeStatusOfOrderCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStatusOfOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangeStatusOfOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                return Result.Failure("Cart not found");
            }

            order.Status = request.Status;
            
            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Cahnged");
        }
    }
}
