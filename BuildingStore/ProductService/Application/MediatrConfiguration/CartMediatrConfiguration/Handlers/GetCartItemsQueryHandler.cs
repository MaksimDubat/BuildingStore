using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Queries;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.MediatrConfiguration.CartMediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение категории по идентификатору.
    /// </summary>
    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, IEnumerable<CartItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCartItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CartItems.GetCartItemsAsync(request.CartId, cancellationToken);

            if(result == null)
            {
                throw new ArgumentNullException("Cart is empty");
            }

            return _mapper.Map<IEnumerable<CartItemDto>>(result);
        }
    }
}
