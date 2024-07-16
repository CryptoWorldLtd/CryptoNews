using AngleSharp;
using CryptoWorld.News.Core.Enumerations;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using CryptоWorld.News.Core.ViewModels.HomePage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly ApplicationDbContext dbContext;
        private readonly UrlForNews urlForNews;

        public NewsService(ApplicationDbContext _dbContext, IOptions<UrlForNews> urlForNewsOptions)
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            homeNews = new List<PageNewsModel>();
            urls = new List<string>();
            dbContext = _dbContext;
            urlForNews = urlForNewsOptions.Value;
        }

        public async Task<List<PageNewsModel>> GetPageNewsModelAsync(List<string> urls)
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
                    PageNewsModel model = new PageNewsModel(title, contentInString, imageUrl, dateOfPublish, rating, region);
                    homeNews.Add(model);
                }
            }

            await AddArticleInDbAsync(homeNews);

            return homeNews;
        }

        public async Task<List<string>> GetNewsUrlsAsync(int pagesCount)
        {

            for (int i = 1; i <= pagesCount; i++)
            {
                var document = await context.OpenAsync($"{urlForNews.MoneyBgUrl}?page={i}");
                var newsUrl = document.QuerySelectorAll(".topic > a");

                if (document == null) { throw new Exception(); }

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

        public async Task<List<PageNewsModel>> GetSortedNewsAsync(
           string category = null,
           string searchTerm = null,
           string region = null,
           NewsSorting sorting = NewsSorting.Latest,
           int currentPage = 1,
           int newsPerPage = 2)
        {
            var newsQuery = dbContext.Articles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                newsQuery = newsQuery.Where(n => n.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                newsQuery = newsQuery.Where(n => n.Region == region);
            }

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

            var totalNewsCount = await dbContext.Articles.CountAsync();

            var news = await newsQuery
                 .Skip((currentPage - 1) * newsPerPage)
                 .Take(newsPerPage)
                 .Select(n => new PageNewsModel(
                     n.Title,
                     n.Content,
                     n.ImageUrl,
                     n.PublicationDate.ToString(),
                     n.Rating,
                     n.Region
                 ))
                 .ToListAsync();

            return news;
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var categories = await dbContext
                .Categories
                .Select(c => c.Name)
                .ToListAsync();

            return categories;
        }

        private async Task AddArticleInDbAsync(List<PageNewsModel> models)
        {
            var category = await GetOrCreateCategory("Crypto");
            var source = await GetOrCreateSource("Money.bg", $"{urlForNews.MoneyBgUrl}");

            List<Article> articles = new List<Article>();
            foreach (var article in homeNews)
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
                bool isValidDate = DateTime.TryParseExact(article.DatePublished, formatDate, CultureInfo.InvariantCulture
                    , DateTimeStyles.None, out DateTime publicationDate
                );

                if (isValidDate)
                {
                    articleModel.PublicationDate = publicationDate;
                }
                else
                {
                    articleModel.PublicationDate = DateTime.MinValue;
                }
                articleModel.SourceId = source.Id;
                articleModel.CategoryId = category.Id;
                if (!dbContext.Articles.Any(a => a.Title == articleModel.Title) &&
                    !dbContext.Articles.Any(a => a.PublicationDate == articleModel.PublicationDate))
                {
                    articles.Add(articleModel);
                }
            }
            await dbContext.Articles.AddRangeAsync(articles);
            await dbContext.SaveChangesAsync();
        }

        private async Task<Source> GetOrCreateSource(string sourceName, string sourceUrl)
        {
            var source = dbContext.Sources.FirstOrDefault(s => s.Name == sourceName);
            if (source == null)
            {
                source = new()
                {
                    Name = sourceName,
                    Url = sourceUrl
                };
                dbContext.Sources.Add(source);
                await dbContext.SaveChangesAsync();
            }

            return source;
        }

        private async Task<Category> GetOrCreateCategory(string categoryName)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
            {
                category = new()
                {
                    Name = categoryName
                };
                dbContext.Categories.Add(category);
                await dbContext.SaveChangesAsync();
            };

            return category;
        }
    }
}