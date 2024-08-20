namespace CryptoWorld.News.Core.Services.News;

using AngleSharp;

using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels;
using CryptoWorld.News.Data.Extension;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;

using Microsoft.Extensions.Options;

using Serilog;

using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

public class RssFeedService : IRssFeedService
{
    private readonly RssFeedSettings _rssSettings;
    private readonly IRepository _repository;
    private readonly INewsService _newsService;
    private readonly IHttpClientFactory _httpClientFactory;

    public RssFeedService(IOptions<RssFeedSettings> rssSettings,
                          IRepository repository,
                          INewsService newsService,
                          IHttpClientFactory httpClientFactory)
    {
        this._rssSettings = rssSettings.Value;
        this._repository = repository;
        this._newsService = newsService;
        this._httpClientFactory = httpClientFactory;
    }

    public async Task<List<RssResponseModel>> GetFeedItemsAsync()
    {
        var feedItems = new List<RssResponseModel>();

        try
        {
            var httpClient = this._httpClientFactory.CreateClient();

            var rssFeedUrls = this._rssSettings?.RssFeedUrls;
            if (rssFeedUrls == null || rssFeedUrls.Count == 0)
            {
                Log.Error("RSS feed URLs are not configured or are empty.");
                return feedItems;
            }

            foreach (var url in rssFeedUrls)
            {
                var response = await httpClient.GetStringAsync(url);
                using var stringReader = new StringReader(response);

                using var reader = XmlReader.Create(url);
                var feed = SyndicationFeed.Load(reader);

                if (feed == null)
                {
                    continue;
                }

                foreach (var item in feed.Items)
                {
                    CreateItem(feedItems, item);
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

    private async Task CreateNewsFromRssFeedAsync(List<RssResponseModel> models)
    {
        foreach (var model in models)
        {
            var source = await _newsService.GetOrCreateSource(model.Copyright, model.Copyright);
            var category = await _newsService.GetOrCreateCategory("Crypto");

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

            if (!_repository.AllReadOnly<Article>().Any(a => a.Title == article.Title))
            {
                await _repository.AddAsync(article);
                await _repository.SaveChangesAsync();
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

            var trustedSources = _rssSettings?.TrustedSources;
            if (trustedSources == null || trustedSources.Count == 0)
            {
                Log.Error("RSS feed URLs are not configured or empty.");
                return string.Empty;
            }

            if (!trustedSources.Contains(uri.Host))
            {
                iframe.Remove();
            }
        }

        string textContent = document.Body.TextContent;
        textContent = Regex.Replace(textContent, @"\s+", " ").Trim();

        return textContent;
    }

    private void CreateItem(List<RssResponseModel> feedItems, SyndicationItem item)
    {
        string contentEncoded = item.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")
                                                      .FirstOrDefault() ?? string.Empty;

        string link = item.Links.FirstOrDefault()?.Uri.ToString();
        string copyright = GetHostFromLink(link);
        string sanitizedContent = SanitizeAndFilterIframes(contentEncoded);
        string sanitizedDescription = SanitizeAndFilterIframes(item.Summary.Text);

        feedItems.Add(new()
        {
            Title = item.Title.Text,
            Link = link,
            Description = sanitizedDescription,
            Content = sanitizedContent,
            PublishDate = item.PublishDate.ToString("dd.MM.yyyy"),
            Copyright = copyright
        });
    }
}
