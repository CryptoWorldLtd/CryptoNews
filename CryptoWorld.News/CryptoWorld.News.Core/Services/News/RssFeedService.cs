using AngleSharp;
using AngleSharp.Html.Parser;
using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using Ganss.Xss;
using Microsoft.Extensions.Options;
using Serilog;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace CryptoWorld.News.Core.Services.News
{
    public class RssFeedService : IRssFeedService
    {
        private readonly List<string> trustedSources = new List<string>
        {
            "www.youtube.com",
            "player.vimeo.com"
        };
        private readonly List<string> rssFeedUrls = new List<string>
        {
            "https://cointelegraph.com/rss",
            "https://feeds.feedburner.com/nigeriabitcoincommunity",
            "https://bitcoincore.org/en/rss.xml"
        };
        private readonly ApplicationDbContext dbContext;
        private readonly INewsService newsService;

        public RssFeedService(ApplicationDbContext _dbContext, INewsService _newsService) 
        {
            dbContext = _dbContext;
            newsService = _newsService;
        }

        public async Task<List<RssResponseModel>> GetFeedItemsAsync()
        {
             var feedItems = new List<RssResponseModel>();

            foreach (var url in rssFeedUrls)
            {
                try
                {
                    using var httpClient = new HttpClient();
                    var response = await httpClient.GetStringAsync(url);

                    using var stringReader = new System.IO.StringReader(response);

                    using var reader = XmlReader.Create(url);
                    var feed = SyndicationFeed.Load(reader);

                    if (feed == null)
                    {
                        continue;
                    }

                    foreach (var item in feed.Items)
                    {
                        string contentEncoded = item.ElementExtensions
                                                   .ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")
                                                   .FirstOrDefault() ?? string.Empty;

                        string link = item.Links.FirstOrDefault()?.Uri.ToString();
                        string copyright = GetHostFromLink(link);
                        string sanitizedContent = SanitizeAndFilterIframes(contentEncoded);
                        string sanitizedDescription = SanitizeAndFilterIframes(item.Summary.Text);
                        
                        feedItems.Add(new RssResponseModel
                        {
                            Title = item.Title.Text,
                            Link = link,
                            Description = sanitizedDescription,
                            Content = sanitizedContent,
                            PublishDate = item.PublishDate.ToString("dd.MM.yyyy"),
                            Copyright = copyright
                        });
                    }

                    await CreateNewsFromRssFeedAsync(feedItems);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error fetching RSS feed: {ex.Message}");
                }
            }

            return feedItems;
        }

        public async Task CreateNewsFromRssFeedAsync(List<RssResponseModel> models)
        {
            foreach(var model in models)
            {
                var source = await newsService.GetOrCreateSource(model.Copyright, model.Copyright);
                var category = await newsService.GetOrCreateCategory("Crypto");

                if (!DateTime.TryParseExact(model.PublishDate, "dd.MM.yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime publicationDate))
                {
                    publicationDate = DateTime.MinValue; 
                }

                var article = new Article
                {
                    Title = model.Title,
                    Link = model.Link,
                    Content = model.Content,
                    PublicationDate = publicationDate,
                    Source = source,
                    Description = model.Description,
                    SourceId = source.Id,
                    Category = category,
                    CreatedOn = DateTime.Now
                };

                if(!dbContext.Articles.Any(a => a.Title == article.Title))
                {
                    await dbContext.Articles.AddAsync(article);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private string GetHostFromLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return string.Empty;
            }

            var uri = new Uri(link);
            return uri.Host;
        }

        private string SanitizeAndFilterIframes(string content)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(content)).Result;

            var iframes = document.QuerySelectorAll("iframe");

            foreach (var iframe in iframes)
            {
                var src = iframe.GetAttribute("src");
                var uri = new Uri(src);
                if (!trustedSources.Contains(uri.Host))
                {
                    iframe.Remove();
                }
            }

            string textContent = document.Body.TextContent;
            textContent = Regex.Replace(textContent, @"\s+", " ").Trim();

            return textContent;
        }
    }
}
