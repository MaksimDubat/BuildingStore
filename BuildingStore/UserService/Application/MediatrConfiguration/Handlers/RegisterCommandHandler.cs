using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды регистрации пользователя.
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork, 
           IMapper mapper)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _authenticationService.RegisterAsync(
               request.Model.Name,
               request.Model.Email,
               request.Model.Password,
               cancellationToken);

            if (user is null)
            {
                return Result<UserDto>.Failure("Wrong registration");
            }

            await _unitOfWork.Users.AddEntityAsync(user, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var profile = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(profile, "registration done");
        }
    }
}
