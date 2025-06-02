using MediatR;
using ProductService.Application.Common;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands
{
    /// <summary>
    /// Модель команды для удаления продукта.
    /// </summary>
    public record DeleteProductCommand(int Id) : IRequest<Result>;
}
