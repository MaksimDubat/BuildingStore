using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Infrastructure.UnitOfWork;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    public class GetProductsWithoutSaleQueryHandle : IRequestHandler<GetProductsWithoutSaleQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsWithoutSaleQueryHandle(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsWithoutSaleQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetProductsWithoutSale(cancellationToken);

            if (products is null)
            {
                return Result<IEnumerable<ProductResponseDto>>.Failure("not found products without sale");
            }
            
            var paginatedProducts = products.ApplyPagination(request.PageNumber, request.PageSize);

            var result = _mapper.Map<IEnumerable<ProductResponseDto>>(products);

            return Result<IEnumerable<ProductResponseDto>>.Success(result, "Products");
        }
    }
}
