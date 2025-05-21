using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение рекомендованных товаров исходя из формы.
    /// </summary>
    public record GetProductsFromFormQuery(int PageNumber, int pageSize, FormDto Form) : IRequest<Result<RecomendationsDto>>;

}
