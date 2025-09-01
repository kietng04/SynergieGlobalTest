using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Services;
using News.Api.Services.Auth;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    private readonly IJwtService _jwtService;

    public AuthController(
        IUserService userService,
        ILogger<AuthController> logger,
        IJwtService jwtService)
    {
        _userService = userService;
        _logger = logger;
        _jwtService = jwtService;
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

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<LoginResponseDto>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var result = await _userService.LoginAsync(request);
            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Login successful",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new ApiResponse<LoginResponseDto>
            {
                Success = false,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user");
            return StatusCode(500, new ApiResponse<LoginResponseDto>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }
    [HttpPost("validate-token")]
    public ActionResult<ApiResponse<ValidateTokenResponseDto>> ValidateToken([FromBody] ValidateTokenRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ValidateTokenResponseDto>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }
        var principal = _jwtService.ValidateToken(request.Token);
        var isValid = principal != null;
        return Ok(new ApiResponse<ValidateTokenResponseDto>
        {
            Success = true,
            Data = new ValidateTokenResponseDto { IsValid = isValid },
            Message = isValid ? "Valid" : "Invalid",
            Timestamp = DateTime.UtcNow
        });
    }
}


