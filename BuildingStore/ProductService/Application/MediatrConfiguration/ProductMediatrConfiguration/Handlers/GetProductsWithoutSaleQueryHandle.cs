using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.UnitOfWork;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    public class GetProductsWithoutSaleQueryHandle : IRequestHandler<GetProductsWithoutSaleQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsWithoutSaleQueryHandle(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetProductsWithoutSaleQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetProductsWithoutSale(cancellationToken);

            if (products == null)
            {
                throw new KeyNotFoundException("not found products without sale");
            }

            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }
    }
}
