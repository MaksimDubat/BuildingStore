using AutoMapper;
using MediatR;
using ProductService.Application.Common;
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Queries;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение категории по идентификатору.
    /// </summary>
    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, Result<IEnumerable<CartItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCartItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<CartItemDto>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.CartItems.GetCartItemsAsync(request.CartId, cancellationToken);

            if(items is null)
            {
                return Result<IEnumerable<CartItemDto>>.Failure("Cart is empty");
            }

            var result = _mapper.Map<IEnumerable<CartItemDto>>(items);

            return Result<IEnumerable<CartItemDto>>.Success(result, "Items");
        }
    }
}
