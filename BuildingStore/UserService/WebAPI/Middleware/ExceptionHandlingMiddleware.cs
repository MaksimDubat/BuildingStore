using Serilog;
using System.Net;

namespace UserService.WebAPI.Middleware
{
    /// <summary>
    /// Обработчик входящих запросов.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = Log.ForContext<ExceptionHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled exception caught in middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statuscode = exception switch
            {
                _ => HttpStatusCode.BadRequest
            };

            var response = new
            {
                error = exception.Message,
                statusCode = (int)statuscode,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statuscode;

            return context.Response.WriteAsJsonAsync(response);
        }

    }
}
