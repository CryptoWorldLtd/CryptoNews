using AngleSharp;
using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Extension;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace CryptoWorld.News.Core.Services.News
{
    public class RssFeedService : IRssFeedService
    {
        private readonly IOptions<RssFeedSettings> _rssSettings;

        private readonly List<string> rssFeedUrls;
        private readonly List<string> trustedSources;
        private readonly IRepository repository;
        private readonly INewsService newsService;
        private readonly IHttpClientFactory httpClientFactory;

        public RssFeedService(IOptions<RssFeedSettings> rssSettings,
            IRepository _repository,
            INewsService _newsService,
            IHttpClientFactory _httpClientFactory)
        {
            rssFeedUrls = rssSettings.Value.RssFeedUrls;
            trustedSources = rssSettings.Value.TrustedSources;
            repository = _repository;  
            newsService = _newsService;
            httpClientFactory = _httpClientFactory;
        }

        public async Task<List<RssResponseModel>> GetFeedItemsAsync()
        {
            var feedItems = new List<RssResponseModel>();

            try
            {
                var httpClient = httpClientFactory.CreateClient();

                if (rssFeedUrls == null || !rssFeedUrls.Any())
                {
                    Log.Error("RSS feed URLs are not configured or are empty.");
                    return feedItems;
                }

                foreach (var url in rssFeedUrls)
                {
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
            }
            catch (HttpRequestException httpEx)
            {
                Log.Error($"HTTP error fetching RSS feed: {httpEx.Message}");
            }
            catch (XmlException xmlEx)
            {
                Log.Error($"XML parsing error: {xmlEx.Message}");
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error fetching RSS feed: : {ex.Message}");
            }

            return feedItems;
        }

        public async Task CreateNewsFromRssFeedAsync(List<RssResponseModel> models)
        {
            foreach (var model in models)
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
                    Description = model.Description,
                    SourceId = source.Id,
                    CategoryId = category.Id,
                    CreatedOn = DateTime.Now
                };

                if (!repository.AllReadOnly<Article>().Any(a => a.Title == article.Title))
                {
                    await repository.AddAsync(article);
                    await repository.SaveChangesAsync();
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
