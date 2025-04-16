using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries
{
    /// <summary>
    /// Модель запроса на получение рекомендованных товаров исходя из формы.
    /// </summary>
    public record GetProductsFromFormQuery(FormDto Form) : IRequest<RecomendationsDto>;

}
