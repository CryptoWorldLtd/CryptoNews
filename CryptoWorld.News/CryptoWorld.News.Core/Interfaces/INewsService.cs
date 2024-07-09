using CryptoWorld.News.Core.Enumerations;
using CryptоWorld.News.Core.ViewModels.Home_Page;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface INewsService
    {
        public Task <List<string>> GetNewsUrlsAsync(int pagesCount);
        public Task <List<PageNewsModel>> GetPageNewsModelAsync(List<string> urls);
        public Task<List<string>> GetCategoriesAsync();
        public Task<List<PageNewsModel>> GetSortedNewsAsync(
           string? category = null,
           string? searchTerm = null,
           NewsSorting sorting = NewsSorting.Soonest,
           int currentPage = 1,
           int newsPerPage = 5);
    }
}