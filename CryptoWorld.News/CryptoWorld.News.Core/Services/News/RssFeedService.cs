using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels;
using Ganss.Xss;
using Serilog;
using System.ServiceModel.Syndication;
using System.Xml;

namespace CryptoWorld.News.Core.Services.News
{
    public class RssFeedService : IRssFeedService
    {
        public List<RssFeedViewModel> GetFeedItems(string url)
        {
            var sanitizer = new HtmlSanitizer();

            try
            {
                using var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);

                if (feed == null)
                {
                    return new List<RssFeedViewModel>();
                }

                var feedItems = new List<RssFeedViewModel>();

                foreach (var item in feed.Items)
                {
                    string contentEncoded = item.ElementExtensions
                                               .ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")
                                               .FirstOrDefault() ?? string.Empty;

                    feedItems.Add(new RssFeedViewModel
                    {
                        Title = item.Title.Text,
                        Link = item.Links.FirstOrDefault()?.Uri.ToString(),
                        Description = sanitizer.Sanitize(item.Summary.Text),
                        Content = sanitizer.Sanitize(contentEncoded),
                        PublishDate = item.PublishDate.ToString("o")
                    });
                }

                return feedItems;
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching RSS feed: {ex.Message}");
                return new List<RssFeedViewModel>();
            }
        }
    }
}
