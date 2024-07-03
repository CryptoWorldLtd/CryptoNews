using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task <IActionResult> HomeNews()
        {
            int pagesCount = 7;
            var model = await homeNewsService.HomePageNews(pagesCount);
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            return Ok(model);
        }
    }
}
