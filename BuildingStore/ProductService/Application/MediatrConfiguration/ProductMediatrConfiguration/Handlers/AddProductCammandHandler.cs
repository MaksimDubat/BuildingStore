using AutoMapper;
using MediatR;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Commands;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для добавления проудкта.
    /// </summary>
    public class AddProductCammandHandler : IRequestHandler<AddProductCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddProductCammandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var exist = await _unitOfWork.Products.AnyAsync(
                x => x.Name == request.Product.Name &&
                x.Description == request.Product.Description &&
                x.Price == request.Product.Price &&
                x.ImageURL == request.Product.ImageURL &&
                x.Amount == request.Product.Amount,
                cancellationToken);
            if (exist)
            {
                return "Already Exist";
            }
            var product = _mapper.Map<Product>(request.Product);
            if (product == null)
            {
                throw new InvalidOperationException("Object is empty");
            }
            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return "Product was added";
        }
    }
}


