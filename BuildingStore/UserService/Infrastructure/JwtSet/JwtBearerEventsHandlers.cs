using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UserService.Infrastructure.JwtSet
{
    /// <summary>
    /// Обработчик события для извлечения токена.
    /// </summary>
    public static class JwtBearerEventsHandlers
    {
        public static Task OnMessageReceived(MessageReceivedContext context)
        {
            var token = context.HttpContext.Request.Cookies["cookies"];

            if (token != null)
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    }
}
