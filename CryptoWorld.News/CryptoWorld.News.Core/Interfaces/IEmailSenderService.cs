
namespace CryptоWorld.News.Core.Interfaces
{
    public interface IEmailSenderService
    {        
        Task SendEmailAsync(string reciever, string username, string body);
    }
}
