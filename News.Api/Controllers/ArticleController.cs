using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Services;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IArticleService _articleService;
    private readonly ILogger<ArticleController> _logger;

    public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
    {
        _articleService = articleService;
        _logger = logger;
    }

    [HttpGet("top-10-articles")]
    public async Task<ActionResult<ApiResponse<List<ArticleResponse>>>> GetTop10Articles(Guid categoryId)
    {
        var articles = await _articleService.GetTop10ArticleByCategoryIdAsync(categoryId);
        var articleResponses = convertArticleEntityToResponse(articles);
        return Ok(new ApiResponse<List<ArticleResponse>> { Success = true, Data = articleResponses, Message = "Fetched top 10 articles", Timestamp = DateTime.UtcNow });
    }
    
    private List<ArticleResponse> convertArticleEntityToResponse(List<Article> articles)
    {
        return articles.Select(a => new ArticleResponse { Id = a.Id, CategoryId = a.CategoryId, Headline = a.Headline, Summary = a.Summary, Content = a.Content, PublicationDate = a.PublicationDate, Source = a.Source, Url = a.Url }).ToList();
    }

    [HttpGet("{articleId}/collections")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<List<CollectionResponseDto>>>> GetCollectionsForArticle(Guid articleId)
    {
        var sub = User?.FindFirst("sub")?.Value ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(sub, out var userId))
        {
            return Unauthorized(new ApiResponse<List<CollectionResponseDto>>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var service = HttpContext.RequestServices.GetRequiredService<ICollectionArticleService>();
            var collections = await service.GetCollectionsByArticleAsync(articleId, userId);
            var data = collections.Select(c => new CollectionResponseDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Ok(new ApiResponse<List<CollectionResponseDto>>
            {
                Success = true,
                Data = data,
                Message = "Collections fetched successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching collections for article {ArticleId}", articleId);
            return StatusCode(500, new ApiResponse<List<CollectionResponseDto>>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}