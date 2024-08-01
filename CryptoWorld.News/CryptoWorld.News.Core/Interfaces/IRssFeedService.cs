using CryptoWorld.News.Core.ViewModels;

namespace CryptoWorld.News.Core.Interfaces
{
    public interface IRssFeedService
    {
        public List<RssFeedViewModel> GetFeedItems(string url);
    }
}
