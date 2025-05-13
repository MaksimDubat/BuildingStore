using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Extensions;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Queries;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение всех пользователей.
    /// </summary>
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync(q => q.ApplyPagination(request.PageNumber, request.PageSize),
                cancellationToken);

            var result = _mapper.Map<IEnumerable<UserDto>>(users);

            return Result<IEnumerable<UserDto>>.Success(result, "Users");
        }
    }
}

