using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Services.Auth;

namespace News.Api.Services.PasswordReset;

public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetStore _store;
    private readonly IEmailSender _emailSender;
    private readonly NewsDbContext _dbContext;
    private readonly IPasswordHashingService _hashing;
    private readonly ILogger<PasswordResetService> _logger;

    public PasswordResetService(
        IPasswordResetStore store,
        IEmailSender emailSender,
        NewsDbContext dbContext,
        IPasswordHashingService hashing,
        ILogger<PasswordResetService> logger)
    {
        _store = store;
        _emailSender = emailSender;
        _dbContext = dbContext;
        _hashing = hashing;
        _logger = logger;
    }

    public async Task RequestAsync(Guid userId, string email, CancellationToken ct = default)
    {
        var code = GenerateSixDigitCode();
        _store.Set(userId, code, TimeSpan.FromMinutes(15), ct);

        var subject = "Password reset code";
        var html = $"<p>Your password reset code is:</p><h2 style='letter-spacing:3px'>{code}</h2><p>This code will expire in 15 minutes.</p>";
        await _emailSender.SendAsync(email, subject, html, ct);
        _logger.LogInformation("Password reset code generated for user {UserId}", userId);
    }

    public async Task ResetAsync(Guid userId, string code, string newPassword, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code is required");
        }
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            throw new ArgumentException("New password is invalid");
        }

        var saved = _store.Get(userId, ct);
        if (string.IsNullOrEmpty(saved) || !string.Equals(saved, code, StringComparison.Ordinal))
        {
            throw new ArgumentException("Invalid or expired code");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct)
            ?? throw new KeyNotFoundException("User not found");

        user.Password = _hashing.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);

        _store.Remove(userId, ct);
        _logger.LogInformation("Password reset for user {UserId}", userId);
    }

    private static string GenerateSixDigitCode()
    {
        var value = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return value.ToString("D6");
    }
}


