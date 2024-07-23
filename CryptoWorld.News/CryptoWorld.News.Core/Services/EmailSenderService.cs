﻿using Microsoft.Extensions.Options;
using CryptoWorld.News.Core.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Serilog;

namespace CryptoWorld.News.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SendGridSettings sendGridSettings;

        public EmailSenderService(IOptions<SendGridSettings> _sendGridSettings)
        {
            sendGridSettings = _sendGridSettings.Value;
        }

        public Task SendEmailAsync(string reciever , string username, string body)
        {
            try
            {
                var sender = "test.2010@abv.bg";

                var apiKey = sendGridSettings.ApiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(sender, "CryptoNews");
                var subject = "CryptoNews Automated Email";
                var to = new EmailAddress(reciever, username);
                var plainTextContent = $"Dear {username} you can click the link to confirm your action! <a href=\"{body}\" class=\"email-button\">Get Started</a>";
                var htmlContent = $"Dear {username} you can click the link to confirm your action! <a href=\"{body}\" class=\"email-button\">Get Started</a>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred during sending email {ex}");
                throw new Exception($"Error in SendEmailAsync {ex}");
            }
        }
    }
}
