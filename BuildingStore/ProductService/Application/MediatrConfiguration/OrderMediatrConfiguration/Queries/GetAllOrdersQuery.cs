using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.OrderMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех заказов.
    /// </summary>
    public record GetAllOrdersQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<OrderDto>>;
 
}
