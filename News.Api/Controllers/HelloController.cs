using Microsoft.AspNetCore.Mvc;
using News.Api.Services;
using News.Api.Services.Auth;
using News.Api.Models.Dtos;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    private readonly IHelloService _helloService;
    private readonly ILogger<HelloController> _logger;
    private readonly IPasswordHashingService _passwordHashingService;

    public HelloController(
        IHelloService helloService, 
        ILogger<HelloController> logger,
        IPasswordHashingService passwordHashingService)
    {
        _helloService = helloService;
        _logger = logger;
        _passwordHashingService = passwordHashingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<HelloResponseDto>>> GetHello()
    {
        try
        {
            var result = await _helloService.GetHelloMessageAsync();
            var passwordHashExample = _passwordHashingService.HashPassword("123456");

            return Ok(new ApiResponse<HelloResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Hello message retrieved successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting hello message");
            return StatusCode(500, new ApiResponse<HelloResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("personalized")]
    public async Task<ActionResult<ApiResponse<HelloResponseDto>>> GetPersonalizedHello([FromBody] HelloRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<HelloResponseDto>
                {
                    Success = false,
                    Message = "Invalid request data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                    Timestamp = DateTime.UtcNow
                });
            }

            _logger.LogInformation("Getting personalized hello message for {Name}", request.Name);
            var result = await _helloService.GetPersonalizedHelloAsync(request.Name);
            
            return Ok(new ApiResponse<HelloResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Personalized hello message created successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument provided: {Message}", ex.Message);
            return BadRequest(new ApiResponse<HelloResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating personalized hello message");
            return StatusCode(500, new ApiResponse<HelloResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
