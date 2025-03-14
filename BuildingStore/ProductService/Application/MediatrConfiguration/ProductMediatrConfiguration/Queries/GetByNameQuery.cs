using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товара по наименованию.
    /// </summary>
    public class GetByNameQuery : IRequest<ProductDto>
    {
        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public string Name { get; set; }
        public GetByNameQuery(string name)
        {
            Name = name;
        }
    }
}
