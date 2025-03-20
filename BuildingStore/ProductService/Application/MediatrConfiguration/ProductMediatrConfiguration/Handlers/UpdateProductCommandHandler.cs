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

            var isDuplicate = await _unitOfWork.Products.IsProductExistOrDuplicateAsync(product, cancellationToken);

            if (isDuplicate)
            {
                return "Duplicate";
            }

            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.CategoryId = request.Product.CategoryId;
            product.Price = request.Product.Price;
            product.ImageURL = request.Product.ImageURL;
            product.Amount = request.Product.Amount;

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return "Product was updated";
        }
    }
}
