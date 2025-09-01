using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Services;

namespace News.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailSender emailSender, ILogger<EmailController> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    [HttpPost("mock-send")]
    public async Task<ActionResult<ApiResponse<object>>> MockSend()
    {
        const string to = "nguyenphantuankiet299@gmail.com";
        const string subject = "Test email from News.API";
        const string body = "<h3>Hello!</h3><p>This is a mock test email.</p>";

        try
        {
            await _emailSender.SendAsync(to, subject, body);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Email sent for: " + to,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email: {Message}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Failed to send email for: " + to,
                Errors = new List<string> { ex.Message },
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


