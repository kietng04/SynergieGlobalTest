using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Services;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserService userService,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<RegisterResponseDto>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var result = await _userService.RegisterAsync(request);
            return Ok(new ApiResponse<RegisterResponseDto>
            {
                Success = true,
                Data = result,
                Message = "User registered successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return Conflict(new ApiResponse<RegisterResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user");
            return StatusCode(500, new ApiResponse<RegisterResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


