using AngleSharp;
using CryptoWorld.News.Core.Enumerations;
using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Extension;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using CryptоWorld.News.Core.ViewModels.HomePage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;
using System.Text;

namespace CryptоWorld.News.Core.Services.News
{
    public class NewsService : INewsService
    {
        private readonly AngleSharp.IConfiguration config;
        private readonly IBrowsingContext context;
        private List<PageNewsModel> homeNews;
        private List<string> urls;
        private readonly UrlForNews urlForNews;
        private readonly IRepository repository;

        public NewsService(IOptions<UrlForNews> urlForNewsOptions, IRepository _repository)
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            homeNews = new List<PageNewsModel>();
            urls = new List<string>();
            urlForNews = urlForNewsOptions.Value;
            repository = _repository;
        }

        public async Task<List<PageNewsModel>> GetPageNewsModelAsync(List<string> urls)
        {
            try
            {
                int pagesCount = 7;
                var newsUrls = await GetNewsUrlsAsync(pagesCount);

                if (newsUrls != null)
                {
                    foreach (var url in newsUrls)
                    {
                        var documentForNews = await context.OpenAsync(url);
                        var title = documentForNews.QuerySelector("header > h1").TextContent;
                        var content = new StringBuilder();
                        var category = string.Empty;
                        var allContentOfNews = documentForNews.QuerySelectorAll(".article-text > p");
                        var rating = 0;
                        var region = string.Empty;

                        foreach (var item in allContentOfNews)
                        {
                            content.AppendLine(item.TextContent);
                        }

                        var imageUrl = documentForNews.QuerySelector(".img-wrapper > .img > img").GetAttribute("src");
                        var dateOfPublish = documentForNews.QuerySelector(".article-info > .time").TextContent.Trim();
                        var contentInString = content.ToString().TrimEnd();
                        PageNewsModel model = new PageNewsModel(title, contentInString, category, imageUrl, dateOfPublish, rating, region);
                        homeNews.Add(model);
                    }
                }

                await AddArticleInDbAsync(homeNews);
                return homeNews;
            }
            catch (Exception ex)
            {
                Log.Error($"Error occurred while processing news! {ex}");
                throw new Exception($"Error in GetPageNewsModelAsync! {ex}");
            }
        }

        public async Task<List<string>> GetNewsUrlsAsync(int pagesCount)
        {
            try
            {
                var urls = new List<string>();

                for (int i = 1; i <= pagesCount; i++)
                {
                    var document = await context.OpenAsync($"{urlForNews.MoneyBgUrl}?page={i}");
                    var newsUrl = document.QuerySelectorAll(".topic > a");
                    if (document == null)
                    {
                        Log.Warning("Problem with document for scrapping");
                    }
                    foreach (var item in newsUrl)
                    {
                        if (item.GetAttribute("href").Contains("kripto"))
                        {
                            urls.Add(item.GetAttribute("href"));
                        }
                    }
                }

                return urls;
            }
            catch (Exception ex)
            {
                Log.Error($"Error occurred while processing pages: {ex.Message}");
                throw new Exception($"Error in GetNewsUrls {ex}");
            }
        }

        public async Task<List<PageNewsModel>> GetSortedNewsAsync(
           string category = null,
           string searchTerm = null,
           string region = null,
           DateTime? startDate = null,
           DateTime? endDate = null,
           NewsSorting sorting = NewsSorting.Latest,
           int currentPage = 1,
           int newsPerPage = 2)
        {
            try
            {
                var newsQuery = repository.AllReadOnly<Article>().AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                    newsQuery = newsQuery.Where(n => n.Category.Name == category);

                if (!string.IsNullOrWhiteSpace(region))
                    newsQuery = newsQuery.Where(n => n.Region == region);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    newsQuery = newsQuery
                        .Where(n =>
                        n.Title.ToLower().Contains(searchTerm.ToLower()) ||
                        n.Content.ToLower().Contains(searchTerm.ToLower()) ||
                        n.Source.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        n.Category.Name.ToLower().Contains(searchTerm.ToLower()) ||
                        n.Region.ToLower().Contains(searchTerm.ToLower()));
                }

                if (startDate.HasValue)
                    newsQuery = newsQuery.Where(n => n.PublicationDate.Date >= startDate.Value);

                if (endDate.HasValue)
                    newsQuery = newsQuery.Where(n => n.PublicationDate.Date <= endDate.Value);

                newsQuery = sorting switch
                {
                    NewsSorting.PublishedPastWeek => newsQuery.Where(n =>
                        n.PublicationDate.Date >= DateTime.Now.Date.AddDays(-7))
                        .OrderByDescending(n => n.PublicationDate),
                    NewsSorting.PublishedPastMonth => newsQuery.Where(n =>
                        n.PublicationDate.Date >= DateTime.Now.Date.AddDays(-30))
                        .OrderByDescending(n => n.PublicationDate),
                    NewsSorting.MostPopular => newsQuery.OrderByDescending(n => n.Rating),
                    NewsSorting.Latest or _ => newsQuery.OrderByDescending(n => n.PublicationDate)
                };

                var totalNewsCount = await repository.AllReadOnly<Article>().CountAsync();

                var news = await newsQuery
                     .Skip((currentPage - 1) * newsPerPage)
                     .Take(newsPerPage)
                     .Select(n => new PageNewsModel(
                         n.Title,
                         n.Content,
                         n.Category.Name,
                         n.ImageUrl,
                         n.PublicationDate.ToString(),
                         n.Rating,
                         n.Region
                     ))
                     .ToListAsync();

                return news;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while getting sorted news.", ex);
                return [];
            }

        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                var categories = await repository.AllReadOnly<Category>()
                .Select(c => c.Name)
                .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                Log.Error($"Problem with method GetCategoriesAsync! {ex}");
                throw new Exception($"Error in method GetCategoriesAsync {ex}");
            }
        }

        private async Task AddArticleInDbAsync(List<PageNewsModel> models)
        {
            var category = await GetOrCreateCategory("Crypto");
            var source = await GetOrCreateSource("Money.bg", $"{urlForNews.MoneyBgUrl}");

            List<Article> articles = new();
            foreach (var article in models)
            {
                var articleModel = new Article()
                {
                    Title = article.Title,
                    Content = article.Content,
                    ImageUrl = article.ImageUrl,
                    SourceId = source.Id,
                    CategoryId = category.Id
                };

                string formatDate = "dd.MM.yyyy HH:mm:ss";
                bool isValidDate = DateTime.TryParseExact(article.DatePublished, formatDate, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime publicationDate);

                articleModel.PublicationDate = isValidDate ? publicationDate : DateTime.MinValue;

                if (!repository.AllReadOnly<Article>().Any(a => a.Title == articleModel.Title &&
                a.PublicationDate == articleModel.PublicationDate))
                {
                    articles.Add(articleModel);
                }
            }

            if (articles.Count > 0)
            {
                await repository.AddRangeAsync(articles);
                await repository.SaveChangesAsync();
            }
        }

        private async Task<Source> GetOrCreateSource(string sourceName, string sourceUrl)
        {
            try
            {
                var source = repository.AllReadOnly<Source>().FirstOrDefault(s => s.Name == sourceName);
                if (source == null)
                {
                    source = new()
                    {
                        Name = sourceName,
                        Url = sourceUrl
                    };

                    await repository.AddAsync(source);
                    await repository.SaveChangesAsync();
                }

                return source;
            }
            catch (Exception ex)
            {
                Log.Error($"Problem with settings of source! {ex}");
                throw new Exception($"Error in GetOrCreateSource {ex}");
            }
        }

        private async Task<Category> GetOrCreateCategory(string categoryName)
        {
            try
            {
                var category = repository.AllReadOnly<Category>().FirstOrDefault(c => c.Name == categoryName);
                if (category == null)
                {
                    category = new()
                    {
                        Name = categoryName
                    };
                    await repository.AddAsync(category);
                    await repository.SaveChangesAsync();
                }

                return category;
            }
            catch (Exception ex)
            {
                Log.Error($"Problem with settings of category! {ex}");
                throw new Exception($"Error in GetOrCreateCategory {ex}");
            }
        }
        public async Task<List<FilterNewsModel>> GetAllNewsForCertainPeriodOfTime(int days)
        {
            var daysAgo = DateTime.Now.AddDays(-days);

            var latestNews = await this.repository.AllReadOnly<Article>()
                .Where(c => c.PublicationDate >= daysAgo)
                .Select(x => new FilterNewsModel()
                {
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Content = x.Content,
                    DatePublished = x.PublicationDate
                })
                .ToListAsync();

            return latestNews;
        }
    }
}