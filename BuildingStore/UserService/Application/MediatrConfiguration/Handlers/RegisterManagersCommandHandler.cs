using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Enums;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды регистрации менеджеров.
    /// </summary>
    public class RegisterManagersCommandHandler : IRequestHandler<RegisterManagersCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public RegisterManagersCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
             IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(RegisterManagersCommand request, CancellationToken cancellationToken)
        {
            var user = await _authenticationService.RegisterAsync(
               request.Model.Name,
               request.Model.Email,
               request.Model.Password,
               cancellationToken);

            if (user == null)
            {
                return Result<UserDto>.Failure("Wrong registration");
            }

            user.Role = UserRole.Manager;

            await _unitOfWork.Users.AddEntityAsync(user, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var profile = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(profile, "registration done");
        }
    }
}
