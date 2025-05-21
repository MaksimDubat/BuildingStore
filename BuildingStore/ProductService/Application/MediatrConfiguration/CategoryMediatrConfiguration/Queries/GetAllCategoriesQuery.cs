using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех категорий.
    /// </summary>
    public record GetAllCategoriesQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<CategoryDto>>>;
}
