using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptоWorld.News.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CryptoWorld.Application.Server.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly INewsService _homeNewsService;
        public NewsController(INewsService homeNewsService)
        {
            _homeNewsService = homeNewsService;
        }

        [HttpGet("home")]
        [AllowAnonymous]
        public async Task<IActionResult> HomeNews()
        {
            try
            {
                int pagesCount = 7;
                var urls = await _homeNewsService.GetNewsUrlsAsync(pagesCount);

                if (urls == null)
                {
                    return BadRequest();
                }

                var model = await _homeNewsService.GetPageNewsModelAsync(urls);
                if (model == null)
                {
                    Log.Warning("No news!");
                    return NotFound();
                }
                else
                {
                    Log.Information("News from home page are loaded successfully!");
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error loading home page! {ex}");
                return BadRequest();
            }
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
				var result = await _homeNewsService.GetAllNewsForCertainPeriodOfTime(days);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("filter")]
        public async Task<IActionResult> GetSortedNewsAsync([FromQuery] FilteredNewsModel news)
        {
            try
            {
                var queryResult = await _homeNewsService.GetSortedNewsAsync(
               news.Category,
               news.SearchTerm,
               news.Region,
               news.StartDate,
               news.EndDate,
               news.Sorting,
               news.CurrentPage,
               FilteredNewsModel.NewsPerPage);
                if (queryResult == null)
                {
                    Log.Information("Not news for searched criteria");
                    return NotFound();
                }
                else
                {
                    Log.Information("HomeNewsService => GetSortedNewsAsync action works normally!");
                    return Ok(queryResult);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error loading news with search criteria! {ex}");
                return BadRequest();
            }
        }
    }
}