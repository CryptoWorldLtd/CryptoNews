using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWorld.Application.Server.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly INewsService _homeNewsService;

        public NewsController(INewsService homeNewsService)
        {
            _homeNewsService = homeNewsService;
        }

        [HttpGet("home")]
        [AllowAnonymous]
        public async Task <IActionResult> HomeNews()
        {
            int pagesCount = 7;
            var urls = await _homeNewsService.GetNewsUrlsAsync(pagesCount);

            if (urls == null)
            {
                return BadRequest();
            }

            var model = await _homeNewsService.GetPageNewsModelAsync(urls);

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            return Ok(model);
        }
    }
}
