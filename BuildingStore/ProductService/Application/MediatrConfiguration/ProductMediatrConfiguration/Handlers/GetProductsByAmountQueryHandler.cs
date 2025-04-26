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
    /// Обработчик запроса для получения продуктов по наличию.
    /// </summary>
    public class GetProductsByAmountQueryHandler : IRequestHandler<GetProductsByAmountQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByAmountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetProductsByAmountQuery request, CancellationToken cancellationToken)
        {
            var specification = new ProductByAmountSpecification();

            var products = await _unitOfWork.Products.GetBySpecificationAsync(specification, cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("Not Found");
            }

            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }
    }
}
