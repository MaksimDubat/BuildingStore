using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение продуктов со скидкой
    /// </summary>
    public class GetProductsWithSaleQueryHandler : IRequestHandler<GetProductsWithSaleQuery, Result<IEnumerable<ProductResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsWithSaleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsWithSaleQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetProductsWithSale(cancellationToken);

            if(products is null)
            {
                return Result<IEnumerable<ProductResponseDto>>.Failure("No products with sale");
            }

            var paginatedProducts = products.ApplyPagination(request.PageNumber, request.PageSize);

            var result = _mapper.Map<IEnumerable<ProductResponseDto>>(paginatedProducts);

            return Result<IEnumerable<ProductResponseDto>>.Success(result, "Products");
        }
    }
}
