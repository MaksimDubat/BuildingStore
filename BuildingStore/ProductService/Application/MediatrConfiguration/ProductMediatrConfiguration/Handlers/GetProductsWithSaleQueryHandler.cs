using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение продуктов со скидкой
    /// </summary>
    public class GetProductsWithSaleQueryHandler : IRequestHandler<GetProductsWithSaleQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsWithSaleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetProductsWithSaleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Products.GetProductsWithSale(cancellationToken);

            if(result == null)
            {
                throw new KeyNotFoundException("No products with sale");
            }

            return _mapper.Map<IEnumerable<ProductResponseDto>>(result); 
        }
    }
}
