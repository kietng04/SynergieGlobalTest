using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Repositories;
using News.Api.Services.PasswordReset;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordResetController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PasswordResetController> _logger;

    public PasswordResetController(
        IPasswordResetService passwordResetService,
        IUserRepository userRepository,
        ILogger<PasswordResetController> logger)
    {
        _passwordResetService = passwordResetService;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpPost("request")]
    public async Task<ActionResult<ApiResponse<object>>> Request([FromBody] PasswordResetRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user != null)
            {
                await _passwordResetService.RequestAsync(user.Id, user.Email);
            }
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "If an account exists for this email, a reset code has been sent",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting password reset");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("confirm")]
    public async Task<ActionResult<ApiResponse<object>>> Confirm([FromBody] PasswordResetConfirmDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid request data",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList(),
                Timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid email or code",
                    Timestamp = DateTime.UtcNow
                });
            }

            await _passwordResetService.ResetAsync(user.Id, request.Code, request.NewPassword);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password has been reset successfully",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message,
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
            _logger.LogError(ex, "Error confirming password reset");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "An error occurred while processing your request",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


