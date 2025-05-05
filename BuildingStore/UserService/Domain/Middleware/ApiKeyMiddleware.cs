using System.Security.Claims;

namespace UserService.Domain.Middleware
{
    /// <summary>
    /// Класс проверки API-ключа.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var referer = context.Request.Headers["Referer"].ToString();
            var origin = context.Request.Headers["Origin"].ToString();

            if ((referer?.Contains("localhost", StringComparison.OrdinalIgnoreCase) ?? false) ||
                (origin?.Contains("localhost", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                await _next(context);
                return;
            }

            var validApiKey = _configuration["ApiKey"];

            if (context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey) &&
                extractedApiKey == validApiKey)

            {
                var claims = new List<Claim>
                {

                    new Claim(ClaimTypes.Name, "System.NotificationService"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("IsSystemUser", "true")
                };

                var identity = new ClaimsIdentity(claims, "ApiKeyAuth");
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}
