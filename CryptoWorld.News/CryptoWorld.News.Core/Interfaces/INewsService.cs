using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.ViewModels.Home_Page;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface INewsService
    {
        public Task <List<string>> GetNewsUrlsAsync(int pagesCount);
        public Task <List<PageNewsModel>> GetPageNewsModelAsync(List<string> urls);

        
    }
}
