using MediatR;
using System.Security.Claims;
using UserService.Application.MediatrConfiguration.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.MediatrConfiguration.Handlers
{
    /// <summary>
    /// Обработчик запроса выхода пользователя.
    /// </summary>
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserProfileCacheService _profileCacheService;
        public LogoutCommandHandler(IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor,
            IUserProfileCacheService profileCacheService)
        {
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
            _profileCacheService = profileCacheService;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId != null && int.TryParse(userId.Value, out var result))
            {
                await _profileCacheService.RemoveProfileAsync(result, cancellationToken);
            }
        
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("cookies");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("fresh-cookies");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("fresh-time-cookies");

        }
    }
}
