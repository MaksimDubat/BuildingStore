using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Queries;

namespace ProductService.Application.MediatrConfiguration.ProductMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение продукта по идентификатору.
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetAsync(request.Id, cancellationToken);

            if (product is null)
            {
                return Result<ProductResponseDto>.Failure("Not found");
            }
            
            var result = _mapper.Map<ProductResponseDto>(product);

            return Result<ProductResponseDto>.Success(result, "Products");
        }
    }
}
