using AutoMapper;
using MediatR;
using UserService.Application.Common;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для входа пользователя.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResult>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public LoginCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork,
            IMapper mapper, IRefreshTokenGenerator refreshTokenGenerator) 
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.SignInAsync(request.Model.Email, request.Model.Password, cancellationToken);

            if(token is null)
            {
                return Result<LoginResult>.Failure("operation invalid");
            }
           
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Model.Email, cancellationToken);

            if(user is null)
            {
                return Result<LoginResult>.Failure("User not found");
            }

            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var result = new LoginResult
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                User = user,
            };

            return Result<LoginResult>.Success(result, "Hello");
        }
    }
}
