using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Queries;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса на получение пользователя по идентификатору.
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
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

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedUser = await _profileCacheService.GetProfileAsync(request.UserId, cancellationToken);

            if (cachedUser != null)
            {
                return cachedUser;
            }

            var result = await _unitOfWork.Users.GetAsync(request.UserId, cancellationToken);

            if (result == null)
            {
                throw new KeyNotFoundException("not found");
            }

            return _mapper.Map<UserDto>(result);
        }
    }
}
