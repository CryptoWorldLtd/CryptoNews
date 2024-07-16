using Microsoft.Extensions.Options;
using CryptoWorld.News.Core.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace CryptoWorld.News.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SendGridSettings _sendGridSettings;

        public EmailSenderService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public Task SendEmailAsync(string reciever , string username, string body)
        {
            var sender = "test.2010@abv.bg";

            var apiKey = _sendGridSettings.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(sender, "CryptoNews");
            var subject = "CryptoNews Automated Email";
            var to = new EmailAddress(reciever, username);
            var plainTextContent = $"Dear {username} you can click the link to confirm your action! <a href=\"{body}\" class=\"email-button\">Get Started</a>";
            var htmlContent = $"Dear {username} you can click the link to confirm your action! <a href=\"{body}\" class=\"email-button\">Get Started</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            return client.SendEmailAsync(msg);
        }
    }
}
