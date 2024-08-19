using CryptoWorld.News.Core.Enumerations;
using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.ViewModels.Home_Page;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface INewsService
    {
        public Task <List<string>> GetNewsUrlsAsync(int pagesCount);
        public Task <List<PageNewsModel>> GetPageNewsModelAsync(List<string> urls);
        public Task<List<string>> GetCategoriesAsync();
        public Task<List<PageNewsModel>> GetSortedNewsAsync(
           string category = null,
           string searchTerm = null,
           string region = null,
           DateTime? startDate = null,
           DateTime? endDate = null,
           NewsSorting sorting = NewsSorting.Latest,
           int currentPage = 1,
           int newsPerPage = 5);
        public Task<List<FilterNewsModel>> GetAllNewsForCertainPeriodOfTime(int days);
        public Task<Source> GetOrCreateSource(string sourceName, string sourceUrl);
        public Task<Category> GetOrCreateCategory(string categoryName);
    }
}