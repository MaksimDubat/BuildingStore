using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Specifications;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение продуктов отсортированных по цене.
    /// </summary>
    public class GetProductsByPriceQueryHandler : IRequestHandler<GetProductsByPriceQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByPriceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByPriceQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByPriceSpecification(request.MinPrice, request.MaxPrice, request.OrderBy);

            if(specification == null)
            {
                throw new ArgumentNullException("Specification is null");
            }

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if(products == null)
            {
                throw new KeyNotFoundException("Not Found");
            }

            IEnumerable<Product> sortedProducts;

            if (request.OrderBy)
            {
                 sortedProducts = products.OrderBy(x => x.Price);
            }
            else
            {
                 sortedProducts = products.OrderByDescending(x => x.Price);
            }

            return _mapper.Map<IEnumerable<ProductDto>>(sortedProducts);
        }
    }
}
