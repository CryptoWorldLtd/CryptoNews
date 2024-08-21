namespace CryptoWorld.News.Core.Interfaces;

using CryptoWorld.News.Core.ViewModels;

public interface IRssFeedService
{
    public Task<List<RssResponseModel>> GetFeedItemsAsync();
}
