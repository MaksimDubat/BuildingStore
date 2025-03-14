using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товара по идентификатору.
    /// </summary>
    public class GetByIdQuery : IRequest<ProductDto>
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int Id { get; set; }
        public GetByIdQuery(int id)
        {
            Id = id;
        }
    }
}
