using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления продукта.
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return "Not found";
            }

            var isDuplicate = await _unitOfWork.Products.AnyAsync(
                x => x.ProductId != request.Id &&
                x.Name == request.Product.Name &&
                x.Description == request.Product.Description &&
                x.Price == request.Product.Price,
                cancellationToken);
            if (isDuplicate)
            {
                return "Duplicate";
            }

            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.Price = request.Product.Price;
            product.ImageURL = request.Product.ImageURL;
            product.Amount = request.Product.Amount;

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return "Product was updated";
        }
    }
}
