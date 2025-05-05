using MediatR;
using System.Security.Claims;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.JwtSet;
using UserService.Infrastructure.RefreshTokenSet;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик команды для обновления токенов.
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(IJwtGenerator jwtGenerator, IRefreshTokenGenerator refreshTokenGenerator,
            IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _jwtGenerator = jwtGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var refresh = _httpContextAccessor.HttpContext?.Request.Cookies["fresh-cookies"];

            if (refresh == null)
            {
                throw new UnauthorizedAccessException("refreshtoken not found");
            }

            var expiresAtString = _httpContextAccessor.HttpContext?.Request.Cookies["fresh-time-cookies"];

            if (expiresAtString == null)
            {
                throw new UnauthorizedAccessException("refreshtoken not found");
            }

            var expires = DateTime.Parse(expiresAtString, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var refreshToken = new RefreshToken
            {
                Refresh = refresh,
                Expires = expires
            };

            var isValid = _refreshTokenGenerator.IsRefreshTokenValid(refreshToken);

            if (!isValid)
            {
                throw new UnauthorizedAccessException("refreshtoken is invalid");
            }

            var userFromCoockie = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if(userFromCoockie == null)
            {
                throw new UnauthorizedAccessException("user not found");
            }

            var result = int.TryParse(userFromCoockie.Value, out var userId);

            if (!result)
            {
                throw new ArgumentException("wrong operation");
            }

            var user = await _unitOfWork.Users.GetAsync(userId, cancellationToken);

            if (user == null)
            {
                throw new KeyNotFoundException("user not found");
            }

            var newAccessToken = _jwtGenerator.GenerateToken(user);

            if(newAccessToken == null)
            {
                throw new UnauthorizedAccessException("wrong generation");
            }

            var newRefreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            if (newRefreshToken == null)
            {
                throw new UnauthorizedAccessException("wrong generation");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("cookies", newAccessToken, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("fresh-cookies", newRefreshToken.Refresh, cookieOptions);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("fresh-time-cookies", newRefreshToken.Expires.ToString("O"), cookieOptions);

        }
    }
}
