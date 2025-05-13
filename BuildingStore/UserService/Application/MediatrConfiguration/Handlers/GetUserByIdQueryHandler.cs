using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Queries;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение пользователя по идентификатору.
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserProfileCacheService _profileCacheService;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserProfileCacheService profileCacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _profileCacheService = profileCacheService;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedUser = await _profileCacheService.GetProfileAsync(request.UserId, cancellationToken);

            if (cachedUser is not null)
            {
                return Result<UserDto>.Success(cachedUser, "User");
            }

            var user = await _unitOfWork.Users.GetAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return Result<UserDto>.Failure("user not found");
            }

            var result = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(result, "User");
        }
    }
}
