using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение всех категорий.
    /// </summary>
    public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
}
