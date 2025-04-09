using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Specifications;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение товаров по наименованию категории.
    /// </summary>
    public class GetProductByCategoryQueryHandler : IRequestHandler<GetProductByCategoryQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByCategorySpecification(request.Name);

            if(specification == null)
            {
                throw new ArgumentNullException("Specification is null");
            }

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("Not Found");
            }

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
