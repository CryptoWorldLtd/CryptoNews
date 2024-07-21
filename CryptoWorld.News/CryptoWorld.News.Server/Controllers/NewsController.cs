using CryptoWorld.News.Data.Models;
using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        [HttpGet("news")]
        [AllowAnonymous]
        public async Task<IActionResult> NewsForCertainPeriod(int days)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var result = await this.homeNewsService.GetAllNewsForCertainPeriodOfTime(days);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
