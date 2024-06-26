using AngleSharp;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using System.Text;

namespace CryptоWorld.News.Core.Services.News
{
    public class HomeNewsService : IHomeNewsService
    {    
        private readonly AngleSharp.IConfiguration config;
        private readonly IBrowsingContext context;
        private List<HomePageNewsModel> _homeNews = new();
        private List<string> _urls = new();
        public HomeNewsService(List<HomePageNewsModel> homeNews, List<string> urls)
        {
            config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            _homeNews = homeNews;
            _urls = urls;
        }

        public async Task<List<HomePageNewsModel>> HomePageNews()
        {
            var document = await context.OpenAsync("https://money.bg/finance/");

            var newsUrl = document.QuerySelectorAll(".topic > a");

            

            foreach (var item in newsUrl)
            {
                if (item.GetAttribute("href").Contains("kripto"))
                {
                    Console.WriteLine(item.GetAttribute("href"));
                }
                
                _urls.Add(item.GetAttribute("href"));
            }
            if (newsUrl != null)
            {
               
                foreach (var url in _urls)
                {
                    var documentForNews = await context.OpenAsync(url);
                    var title = documentForNews.QuerySelector("header > h1").ToString();

                    StringBuilder content = new StringBuilder();

                    var allContentOfNews =  documentForNews.QuerySelectorAll(".article-text > p");
                    foreach (var item in allContentOfNews) 
                    {
                        content.AppendLine(documentForNews.TextContent);
                    }

                    var imageUrl = documentForNews.QuerySelector(".img-wrapper > .img > img").GetAttribute("src");

                    var dateOfPublish = documentForNews.QuerySelector(".article-info > .time").ToString().Trim();
                    
                   var contentInString = content.ToString().TrimEnd();

                    HomePageNewsModel model = new(title, dateOfPublish, imageUrl,contentInString);

                    _homeNews.Add(model);
                }
                
            }

            return _homeNews;
        }
    }
}
