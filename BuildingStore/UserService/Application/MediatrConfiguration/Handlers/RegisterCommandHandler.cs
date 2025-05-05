using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды регистрации пользователя.
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileCacheService _profileCacheService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, 
            IUserProfileCacheService profileCacheService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _profileCacheService = profileCacheService;
            _mapper = mapper;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
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

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var profile = _mapper.Map<UserDto>(user);
            await _profileCacheService.SetProfileAsync(user.Id, profile, TimeSpan.FromHours(3), cancellationToken);

        }
    }
}
