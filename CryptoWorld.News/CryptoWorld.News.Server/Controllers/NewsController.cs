using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CryptoWorld.Application.Server.Controllers
{
    public class NewsController : BaseApiController
    {
       
        private readonly INewsService homeNewsService;

        public NewsController(INewsService _homeNewsService)
        {
            homeNewsService = _homeNewsService;
        }

        [HttpGet("home")]
        [AllowAnonymous]
        public async Task<IActionResult> HomeNews()
        {
            int pagesCount = 7;
            var urls = await homeNewsService.GetNewsUrlsAsync(pagesCount);

            if (urls == null)
            {
                return BadRequest();
            }

            var model = await homeNewsService.GetPageNewsModelAsync(urls);

            

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Log.Information("HomeNewsService => HomeNews action => HomeNews action works normally!" );
            return Ok(model);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetSortedNewsAsync([FromQuery] FilteredNewsModel news)
        {
            var queryResult = await homeNewsService.GetSortedNewsAsync(
                news.Category,
                news.SearchTerm,
                news.Region,
                news.StartDate,
                news.EndDate,
                news.Sorting,
                news.CurrentPage,
                FilteredNewsModel.NewsPerPage);

            Log.Information("HomeNewsService => GetSortedNewsAsync action works normally!");
            return Ok(queryResult);
        }
    }
}