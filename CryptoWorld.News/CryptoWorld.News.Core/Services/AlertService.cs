using CryptoWorld.News.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CryptoWorld.News.Core.Services
{
    public class AlertService : IAlertService
    {
        private readonly IEmailSenderService emailSenderService;
        private readonly ILogger logger;

        public AlertService(IEmailSenderService _emailSenderService, ILogger<AlertService> _logger)
        {
            emailSenderService = _emailSenderService;
            logger = _logger;
        }

        public async Task SendCriticalErrorAlertAsync(string subject, Exception ex)
        {
            var plainTextContent = $"A critical error occurred: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}";
            var htmlContent = $"<strong>A critical error occurred:</strong><br>{ex.Message}<br><br><strong>Stack Trace:</strong><br>{ex.StackTrace.Replace("\n", "<br>")}";

            await emailSenderService.SendEmailAsync("admin@cryptoWorldNews.com", "System Administrator", plainTextContent, htmlContent);
        }
    }
}