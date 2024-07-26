namespace CryptoWorld.News.Core.Interfaces
{
    public interface IAlertService
    {
        Task SendCriticalErrorAlertAsync(string subject, Exception ex);
    }
}
