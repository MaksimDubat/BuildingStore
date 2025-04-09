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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException("not found");
            }

            var requestProduct = _mapper.Map<Product>(request.Product);

            if (requestProduct == null)
            {
                throw new ArgumentNullException("object is empty");
            }

            var isDuplicate = await _unitOfWork.Products.IsProductExistOrDuplicateAsync(requestProduct, cancellationToken);

            if (isDuplicate)
            {
                throw new ArgumentException("Already exist");
            }

            product.Name = requestProduct.Name;
            product.Description = requestProduct.Description;
            product.CategoryId = requestProduct.CategoryId;
            product.Price = requestProduct.Price;
            product.ImageURL = requestProduct.ImageURL;
            product.Amount = requestProduct.Amount;

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}
