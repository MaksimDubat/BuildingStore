using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение товара по наименованию.
    /// </summary>
    public class GetProductByNameQuery : IRequest<ProductDto>
    {
        /// <summary>
        /// Наименование продукта.
        /// </summary>
        public string Name { get; set; }
        public GetProductByNameQuery(string name)
        {
            Name = name;
        }
    }
}
