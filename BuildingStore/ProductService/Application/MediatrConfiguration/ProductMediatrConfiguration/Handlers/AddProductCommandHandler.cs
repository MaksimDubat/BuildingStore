using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Application.Services;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для добавления продукта.
    /// </summary>
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public AddProductCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ImageService imageService, 
            IWebHostEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
            _environment = environment;
        }

        public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.Product);

            if (product == null)
            {
                throw new ArgumentNullException("object is empty");
            }

            var exist = await _unitOfWork.Products.IsProductExistOrDuplicateAsync(product, cancellationToken);

            if (exist)
            {
                throw new ArgumentException("Already Exist");
            }

            if(request.Product.Image != null)
            {
                var imagePath = await _imageService.HandleImageAsync(
                    request.Product.Image, 
                    Path.Combine(_environment.WebRootPath, "images", "products"),
                    cancellationToken
                );

                product.Image = imagePath;
            }

            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

        }
    }
}


