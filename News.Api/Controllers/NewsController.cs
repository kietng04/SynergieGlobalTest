using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Services;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly INewsApiService _newsApiService;
    private readonly ILogger<NewsController> _logger;

    public NewsController(INewsApiService newsApiService, ILogger<NewsController> logger)
    {
        _newsApiService = newsApiService;
        _logger = logger;
    }

    [HttpPost("sync")]
    public async Task<ActionResult<ApiResponse<object>>> Sync()
    {
        try
        {
            await _newsApiService.SyncTopNewsToDatabase();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Top news sync executed successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing top news");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while syncing top news",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


