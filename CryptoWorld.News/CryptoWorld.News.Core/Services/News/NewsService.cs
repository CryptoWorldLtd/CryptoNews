﻿using AngleSharp;
using CryptoWorld.News.Data;
using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
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
        public NewsService( ApplicationDbContext _dbContext)
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            homeNews = new List<PageNewsModel>();
            urls = new List<string>();
            dbContext = _dbContext;
        }

        public async Task<List<PageNewsModel>> HomePageNews(int pagesCount = 7)
        {

            for (int i = 1; i <= pagesCount; i++)
            {
                var document = await context.OpenAsync($"https://money.bg/finance?page={i}");
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

            if (urls != null)
            {

                foreach (var url in urls)
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
                var category = await GetOrCreateCategory("Crypto");
                var source = await GetOrCreateSource("Money.bg", "https://money.bg/finance/");

                List<Article> articles = new List<Article>();
                foreach (var article in homeNews)
                {
                    var articleModel = new Article();
                    articleModel.Title = article.Title;
                    articleModel.Content = article.Content;
                    articleModel.ImageUrl = article.ImageUrl;
                    DateTime publicationDate;
                    string format = "dd.MM.yyyy HH:mm:ss";
                    bool isValidDate = DateTime.TryParseExact(article.DatePublished, format, CultureInfo.InvariantCulture
                        , DateTimeStyles.None, out publicationDate
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
            return homeNews;
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
