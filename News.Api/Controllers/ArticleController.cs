using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Services;

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
}