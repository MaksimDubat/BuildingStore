using System.Net;

namespace ProductService.Infrastructure.Middleware
{
    /// <summary>
    /// Обработчик входящих запросов.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statuscode = exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                ArgumentNullException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
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
