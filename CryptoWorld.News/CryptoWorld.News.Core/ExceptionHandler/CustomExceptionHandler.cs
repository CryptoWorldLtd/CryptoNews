using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using Serilog;

namespace CryptoWorld.News.Core.ExceptionHandler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> _logger)
        {
            logger  = _logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string errorMessage) = exception switch
            {
                ForbiddenException => (403, null),
                BadRequestException badRequestException => (400, badRequestException.Message),
                NotFoundException notFoundException => (404, notFoundException.Message),
                _ => (500, "Something went wrong")
            };
            logger.LogError(exception, exception.Message);
            httpContext.Response.StatusCode = statusCode;
           await httpContext.Response.WriteAsJsonAsync(errorMessage);

            return true;
        }
    }
}
