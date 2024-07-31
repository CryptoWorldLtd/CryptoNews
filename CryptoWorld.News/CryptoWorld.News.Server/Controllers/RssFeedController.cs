using CryptoWorld.News.Core.Services.News;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWorld.Application.Server.Controllers
{
    public class RssFeedController : BaseApiController
    {
        private readonly RssFeedService _rssFeedService;

        public RssFeedController(RssFeedService rssFeedService)
        {
            _rssFeedService = rssFeedService;
        }

        [HttpGet("rssFeed")]
        public IActionResult GetFeed([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("URL is requred");
            }

            try
            {
                var items =  _rssFeedService.GetFeedItems(url);
                var result = items.Select(item => new {
                    item.Title,
                    item.Link,
                    item.Description,
                    item.Content,
                    item.PublishDate
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception 
                Console.WriteLine($"Error in GetFeed: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
