using AngleSharp;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using CryptоWorld.News.Core.ViewModels.HomePage;
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
        public NewsService( ApplicationDbContext _dbContext, IOptions<UrlForNews> urlForNewsOptions)
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
                    foreach (var item in allContentOfNews)
                    {
                        content.AppendLine(item.TextContent);
                    }

                    var imageUrl = documentForNews.QuerySelector(".img-wrapper > .img > img").GetAttribute("src");
                    var dateOfPublish = documentForNews.QuerySelector(".article-info > .time").TextContent.Trim();
                    var contentInString = content.ToString().TrimEnd();
                    PageNewsModel model = new PageNewsModel(title, contentInString, imageUrl, dateOfPublish);
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
        public async Task AddArticleInDbAsync(List<PageNewsModel> models)
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
                if (!articles.Any(a => a.Title == articleModel.Title) &&
                    !articles.Any(a => a.PublicationDate == articleModel.PublicationDate))
                {
                    articles.Add(articleModel);
                   
                }
            }
           await dbContext.AddRangeAsync(articles);
            await dbContext.SaveChangesAsync();
        }
        private async Task<Source> GetOrCreateSource (string sourceName , string sourceUrl)
        {
            Source source = new()
            {
                Name = sourceName,
                Url = sourceUrl
            };
            dbContext.Add(source);
            await dbContext.SaveChangesAsync();

            return source;
        }   
        private async Task<Category> GetOrCreateCategory(string categoryName)
        {
            Category category = new()
            {
                Name = categoryName ,
                
            };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            return category;
        }  
    }
}
    

