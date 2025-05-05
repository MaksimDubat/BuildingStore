using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Enums;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды регистрации менеджеров.
    /// </summary>
    public class RegisterManagersCommandHandler : IRequestHandler<RegisterManagersCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserProfileCacheService _profileCacheService;
        private readonly IMapper _mapper;

        public RegisterManagersCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
            IUserProfileCacheService profileCacheService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _profileCacheService = profileCacheService;
            _mapper = mapper;
        }

        public async Task Handle(RegisterManagersCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            if (model.Password != model.ConfirmPassword)
            {
                throw new ArgumentException("Password dont matched");
            }

            var user = await _authenticationService.RegisterAsync(
               request.Model.Name,
               request.Model.Email,
               request.Model.Password,
               cancellationToken);

            user.Role = UserRole.Manager;

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var profile = _mapper.Map<UserDto>(user);
            await _profileCacheService.SetProfileAsync(user.Id, profile, TimeSpan.FromHours(3), cancellationToken);
        }
    }
}
