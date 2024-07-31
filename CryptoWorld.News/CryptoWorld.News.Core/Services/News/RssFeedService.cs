using CryptoWorld.News.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CryptoWorld.News.Core.Services.News
{
    public class RssFeedService
    {
        public List<RssFeedViewModel> GetFeedItems(string url)
        {
            try
            {
                using var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);

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
                        Description = item.Summary.Text,
                        Content = contentEncoded,
                        PublishDate = item.PublishDate.ToString("o")
                    });
                }

                return feedItems;
            }
            catch (Exception ex)
            {
                // Log the exception (for production use a logging framework)
                Console.WriteLine($"Error fetching RSS feed: {ex.Message}");
                return new List<RssFeedViewModel>();
            }
        }
    }
}
