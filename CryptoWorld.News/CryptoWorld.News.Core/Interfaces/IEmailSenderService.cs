namespace CryptoWorld.News.Core.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(
           string reciever,
           string username,
           string plainTextContent,
           string htmlContent);
    }
}