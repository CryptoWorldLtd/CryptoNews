using CryptoWorld.News.Core.ViewModels;

namespace CryptoWorld.News.Core.Interfaces
{
    public interface IRssFeedService
    {
        public Task<List<RssResponseModel>> GetFeedItemsAsync();
    }
}
