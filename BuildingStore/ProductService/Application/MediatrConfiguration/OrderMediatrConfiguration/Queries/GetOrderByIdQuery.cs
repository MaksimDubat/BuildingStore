using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение заказа по идентифкатору.
    /// </summary>
    public record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto>;

}
