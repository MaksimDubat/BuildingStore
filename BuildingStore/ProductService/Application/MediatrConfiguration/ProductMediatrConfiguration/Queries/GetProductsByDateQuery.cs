using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товаров по новизне за опредленный период.
    /// </summary>
    public record GetProductsByDateQuery(int PageNumber, 
        int PageSize, 
        int DayAgo, 
        bool OrderBy) 
        : IRequest<Result<IEnumerable<ProductResponseDto>>>;
}
