using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CryptoWorld.Application.Server.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly ILogger logger;
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

            return Ok(model);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetSortedNewsAsync([FromQuery] FilteredNewsModel news)
        {
            var queryResult = await homeNewsService.GetSortedNewsAsync(
                news.Category,
                news.SearchTerm,
                news.Sorting,
                news.CurrentPage,
                FilteredNewsModel.NewsPerPage);

            return Ok(queryResult);
        }
    }
}