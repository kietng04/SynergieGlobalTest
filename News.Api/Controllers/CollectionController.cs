using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Services;

using System.Security.Claims;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;
    private readonly ICollectionArticleService _collectionArticleService;
    private readonly ILogger<CollectionController> _logger;

    public CollectionController(ICollectionService collectionService, ICollectionArticleService collectionArticleService, ILogger<CollectionController> logger)
    {
        _collectionService = collectionService;
        _collectionArticleService = collectionArticleService;
        _logger = logger;
    }

    [HttpDelete("{collectionId}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid collectionId)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            await _collectionService.DeleteAsync(collectionId, userId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Collection deleted successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting collection");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPut("{collectionId}")]
    public async Task<ActionResult<ApiResponse<CollectionResponseDto>>> Update(Guid collectionId, [FromBody] UpdateCollectionRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var updated = await _collectionService.UpdateAsync(collectionId, userId, request.Name, request.Description ?? string.Empty);
            var data = new CollectionResponseDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                Name = updated.Name,
                Description = updated.Description,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };

            return Ok(new ApiResponse<CollectionResponseDto>
            {
                Success = true,
                Data = data,
                Message = "Collection updated successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating collection");
            return StatusCode(500, new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("{collectionId}/articles")]
    public async Task<ActionResult<ApiResponse<List<ArticleResponse>>>> GetArticles(Guid collectionId)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<List<ArticleResponse>>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var articles = await _collectionArticleService.GetArticlesByCollectionAsync(collectionId, userId);
            var data = articles.Select(a => new ArticleResponse
            {
                Id = a.Id,
                CategoryId = a.CategoryId,
                Headline = a.Headline,
                Summary = a.Summary,
                Content = a.Content,
                PublicationDate = a.PublicationDate,
                Source = a.Source,
                Url = a.Url
            }).ToList();

            return Ok(new ApiResponse<List<ArticleResponse>>
            {
                Success = true,
                Data = data,
                Message = "Articles fetched successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<List<ArticleResponse>>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new ApiResponse<List<ArticleResponse>>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching collection articles");
            return StatusCode(500, new ApiResponse<List<ArticleResponse>>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CollectionResponseDto>>> Create([FromBody] CreateCollectionRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var created = await _collectionService.CreateAsync(userId, request.Name, request.Description ?? string.Empty);
            var data = new CollectionResponseDto
            {
                Id = created.Id,
                UserId = created.UserId,
                Name = created.Name,
                Description = created.Description,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };

            return Ok(new ApiResponse<CollectionResponseDto>
            {
                Success = true,
                Data = data,
                Message = "Collection created successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating collection");
            return StatusCode(500, new ApiResponse<CollectionResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CollectionResponseDto>>>> GetMine()
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<List<CollectionResponseDto>>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        var items = await _collectionService.GetByUserAsync(userId);
        var data = items.Select(c => new CollectionResponseDto
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

    [HttpPost("{collectionId}/articles/{articleId}")]
    public async Task<ActionResult<ApiResponse<CollectionArticleResponseDto>>> AddArticle(Guid collectionId, Guid articleId)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<CollectionArticleResponseDto>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var created = await _collectionArticleService.AddAsync(collectionId, articleId, userId);
            var data = new CollectionArticleResponseDto
            {
                Id = created.Id,
                CollectionId = created.CollectionId,
                ArticleId = created.ArticleId,
                SavedAt = created.SavedAt
            };

            return Ok(new ApiResponse<CollectionArticleResponseDto>
            {
                Success = true,
                Data = data,
                Message = "Article added to collection",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<CollectionArticleResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new ApiResponse<CollectionArticleResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return Conflict(new ApiResponse<CollectionArticleResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding article to collection");
            return StatusCode(500, new ApiResponse<CollectionArticleResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{collectionId}/articles/{articleId}")]
    public async Task<ActionResult<ApiResponse<object>>> RemoveArticle(Guid collectionId, Guid articleId)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            await _collectionArticleService.RemoveArticleAsync(collectionId, articleId, userId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Article removed from collection",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing article from collection");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var sub = User?.FindFirst("sub")?.Value ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(sub, out var userId) ? userId : Guid.Empty;
    }
}


