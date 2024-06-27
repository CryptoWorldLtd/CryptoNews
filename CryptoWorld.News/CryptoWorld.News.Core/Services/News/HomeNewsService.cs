using AngleSharp;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace CryptоWorld.News.Core.Services.News
{
    public class HomeNewsService : IHomeNewsService
    {    
        private readonly AngleSharp.IConfiguration config;
        private readonly IBrowsingContext context;
        private List<HomePageNewsModel> homeNews;
        private List<string> urls;
        private readonly ApplicationDbContext dbContext;
        public HomeNewsService( ApplicationDbContext _dbContext)
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            homeNews = new List<HomePageNewsModel>();
            urls = new List<string>();
            dbContext = _dbContext;
        }

        public async Task<List<HomePageNewsModel>> HomePageNews()
        {
            var document = await context.OpenAsync("https://money.bg/finance/");

            var newsUrl = document.QuerySelectorAll(".topic > a");

            

            foreach (var item in newsUrl)
            {
                if (item.GetAttribute("href").Contains("kripto"))
                {
                    urls.Add(item.GetAttribute("href"));
                }
                
                
            }
            if (urls != null)
            {
               
                foreach (var url in urls)
                {
                    var documentForNews = await context.OpenAsync(url);
                    var title = documentForNews.QuerySelector("header > h1").TextContent;

                    StringBuilder content = new StringBuilder();

                    var allContentOfNews =  documentForNews.QuerySelectorAll(".article-text > p");
                    foreach (var item in allContentOfNews) 
                    {
                        content.AppendLine(item.TextContent);
                    }

                    var imageUrl = documentForNews.QuerySelector(".img-wrapper > .img > img").GetAttribute("src");

                    var dateOfPublish = documentForNews.QuerySelector(".article-info > .time").TextContent.Trim();
                    
                   var contentInString = content.ToString().TrimEnd();

                    HomePageNewsModel model = new HomePageNewsModel(title, contentInString, imageUrl, dateOfPublish);

                    homeNews.Add(model);
                }

                //List<Article> articles = new List<Article>();
                //foreach (var article in homeNews)
                //{
                //    Article articleModel = new Article();
                //    articleModel.Title = article.Title;
                //    articleModel.Content = article.Content;
                //    articleModel.ImageUrl = article.ImageUrl;
                //    DateTime publicationDate;
                //    string format = "dd.MM.yyyy HH:mm:ss";
                //    bool isValidDate = DateTime.TryParseExact(article.DatePublished,format,CultureInfo.InvariantCulture
                //        ,DateTimeStyles.None,out publicationDate
                //    );

                //    if (isValidDate)
                //    {
                //        articleModel.PublicationDate = publicationDate;
                //    }
                //    else
                //    {
                        
                //        articleModel.PublicationDate = DateTime.MinValue; 
                //    }
                //    articles.Add(articleModel);
                //}

               //await dbContext.AddRangeAsync(articles);
               // await dbContext.SaveChangesAsync();
            }

            return homeNews;
        }
    }
}
