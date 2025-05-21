using MediatR;
using ProductService.Application.Common;
using ProductService.Domain.Enums;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды изменения статуса заказа.
    /// </summary>
    public class ChangeStatusOfOrderCommand : IRequest<Result>
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор статуса.
        /// </summary>
        public OrderStatus Status { get; set; }

        public ChangeStatusOfOrderCommand(int orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }
    }
}
