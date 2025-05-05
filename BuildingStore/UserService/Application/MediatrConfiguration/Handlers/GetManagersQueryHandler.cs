using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение менеджеров.
    /// </summary>
    public class GetManagersQueryHandler : IRequestHandler<GetManagersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetManagersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetManagersQuery request, CancellationToken cancellationToken)
        {
            var managers = await _unitOfWork.Users.GetManagersAsync(cancellationToken);

            if (managers == null)
            {
                throw new KeyNotFoundException("Not found");
            }

            return _mapper.Map<IEnumerable<UserDto>>(managers);
        }
    }
}
