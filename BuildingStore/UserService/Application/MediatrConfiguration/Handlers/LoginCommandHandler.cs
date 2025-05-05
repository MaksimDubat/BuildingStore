using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для входа пользователя.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileCacheService _userProfileCacheService;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public LoginCommandHandler(IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor, 
            IUnitOfWork unitOfWork, IUserProfileCacheService userProfileCacheService,
            IMapper mapper, IRefreshTokenGenerator refreshTokenGenerator) 
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _userProfileCacheService = userProfileCacheService;
            _mapper = mapper;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.SignInAsync(request.Model.Email, request.Model.Password, cancellationToken);

            if(token == null)
            {
                throw new UnauthorizedAccessException("operation invalid");
            }
           
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Model.Email, cancellationToken);

            if(user == null)
            {
                throw new KeyNotFoundException("Email not found");
            }

            var cachedProfile = await _userProfileCacheService.GetProfileAsync(user.Id, cancellationToken);

            if(cachedProfile == null)
            {
                var profile = _mapper.Map<UserDto>(user);
                await _userProfileCacheService.SetProfileAsync(profile.Id, profile, TimeSpan.FromHours(3), cancellationToken);
            }

            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("cookies", token, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("fresh-cookies", refreshToken.Refresh, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("fresh-time-cookies", refreshToken.Expires.ToString("O"), cookieOptions);

        }
    }
}
