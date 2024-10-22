﻿namespace CryptoWorld.Application.Server.Controllers;

using CryptoWorld.News.Core.Interfaces;
using CryptoWorld.News.Core.ViewModels.HomePage;
using CryptоWorld.News.Core.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Serilog;
public class NewsController : BaseApiController
{

    private readonly INewsService _homeNewsService;
    private readonly IRssFeedService _rssFeedService;

    public NewsController(INewsService homeNewsService, IRssFeedService rssFeedService)
    {
        this._homeNewsService = homeNewsService;
        this._rssFeedService = rssFeedService;
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
    [Authorize]
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
    [Authorize]
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

    [HttpGet("rssFeed")]
    public async Task<IActionResult> GetRssFeed()
    {

        try
        {
            var items = await _rssFeedService.GetFeedItemsAsync();
            var result = items.Select(item => new
            {
                item.Title,
                item.Link,
                item.Description,
                item.Content,
                item.PublishDate,
                item.Copyright
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetFeed: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}