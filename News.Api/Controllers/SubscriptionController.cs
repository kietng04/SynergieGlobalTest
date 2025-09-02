using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Services;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly IUserSubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(IUserSubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SubscriptionResponseDto>>>> GetMySubscriptions()
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
        {
            return Unauthorized(new ApiResponse<List<SubscriptionResponseDto>>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        var subs = await _subscriptionService.GetByUserAsync(userId);
        var data = subs.Select(s => new SubscriptionResponseDto
        {
            Id = s.Id,
            CategoryId = s.CategoryId,
            EmailFrequency = s.EmailFrequency,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        return Ok(new ApiResponse<List<SubscriptionResponseDto>>
        {
            Success = true,
            Data = data,
            Message = "Subscriptions fetched successfully",
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SubscriptionResponseDto>>> Add([FromBody] CreateSubscriptionRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<SubscriptionResponseDto>
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
            return Unauthorized(new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var sub = await _subscriptionService.AddAsync(userId, request.CategoryId, request.EmailFrequency, request.IsActive);
            var data = new SubscriptionResponseDto
            {
                Id = sub.Id,
                CategoryId = sub.CategoryId,
                EmailFrequency = sub.EmailFrequency,
                IsActive = sub.IsActive,
                CreatedAt = sub.CreatedAt,
                UpdatedAt = sub.UpdatedAt
            };

            return Ok(new ApiResponse<SubscriptionResponseDto>
            {
                Success = true,
                Data = data,
                Message = "Subscription created successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return Conflict(new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription");
            return StatusCode(500, new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{categoryId}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid categoryId)
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
            await _subscriptionService.RemoveAsync(userId, categoryId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Subscription deleted successfully",
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting subscription");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPatch("{categoryId}")]
    public async Task<ActionResult<ApiResponse<SubscriptionResponseDto>>> Update(Guid categoryId, [FromBody] UpdateSubscriptionRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<SubscriptionResponseDto>
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
            return Unauthorized(new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = "Unauthorized",
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var updated = await _subscriptionService.UpdateAsync(userId, categoryId, request.EmailFrequency, request.IsActive);
            var data = new SubscriptionResponseDto
            {
                Id = updated.Id,
                CategoryId = updated.CategoryId,
                EmailFrequency = updated.EmailFrequency,
                IsActive = updated.IsActive,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
            return Ok(new ApiResponse<SubscriptionResponseDto>
            {
                Success = true,
                Data = data,
                Message = "Subscription updated successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating subscription");
            return StatusCode(500, new ApiResponse<SubscriptionResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    private Guid GetUserIdFromClaims()
    {
        var sub = User?.FindFirst("sub")?.Value ?? User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(sub, out var userId) ? userId : Guid.Empty;
    }
}
