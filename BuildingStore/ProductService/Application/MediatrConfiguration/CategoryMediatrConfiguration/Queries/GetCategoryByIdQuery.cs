using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение категории по идентификатору.
    /// </summary>
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;

}
