using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.CategoryMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение категории по идентификатору.
    /// </summary>
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int Id { get; set; }
        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
