using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using Serilog;

namespace CryptoWorld.News.Core.ExceptionHandler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        private readonly IAlertService alertService;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> _logger, IAlertService _alertService)
        {
            logger  = _logger;
            alertService = _alertService;
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

            if (statusCode == 500)
                await alertService.SendCriticalErrorAlertAsync("Critical Error", exception);

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(errorMessage);

            return true;
        }
    }
}
