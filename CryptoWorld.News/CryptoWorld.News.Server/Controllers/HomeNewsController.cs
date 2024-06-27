using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWorld.Application.Server.Controllers
{
    public class HomeNewsController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IHomeNewsService homeNewsService;

        public HomeNewsController(IHomeNewsService _homeNewsService)
        {
            homeNewsService = _homeNewsService;
        }

        [HttpGet("home")]
        [AllowAnonymous]
        public async Task <IActionResult> HomeNews()
        {
            var model = await homeNewsService.HomePageNews();

            if (!ModelState.IsValid) 
            {
                return BadRequest();
            
            }

            return Ok(model);
        }
    }
}
