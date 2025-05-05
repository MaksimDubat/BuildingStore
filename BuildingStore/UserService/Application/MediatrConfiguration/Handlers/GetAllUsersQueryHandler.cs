using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение всех пользователей.
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Users.GetAllAsync(cancellationToken);

            if(result == null)
            {
                throw new ArgumentNullException("not found");
            }

            return _mapper.Map<IEnumerable<UserDto>>(result);
        }
    }
}
