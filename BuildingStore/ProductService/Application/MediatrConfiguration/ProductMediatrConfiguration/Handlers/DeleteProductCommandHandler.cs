using MediatR;
using ProductService.Application.Common;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.Services;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для удаления продукта.
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ImageService _imageService;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ImageService service)
        {
            _unitOfWork = unitOfWork;
            _imageService = service;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.Id, cancellationToken);

            if(product is null)
            {
                return Result.Failure("Not found");
            }

            if (!string.IsNullOrEmpty(product.Image))
            {
                _imageService.DeleteImage(product.Image);
            }

            await _unitOfWork.Products.DeleteAsync(product.ProductId, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result.Success("Deleted");
        }
    }
}
