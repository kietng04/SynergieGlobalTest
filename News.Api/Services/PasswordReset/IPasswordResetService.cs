namespace News.Api.Services.PasswordReset;

public interface IPasswordResetService
{
    Task RequestAsync(Guid userId, string email, CancellationToken ct = default);
    Task ResetAsync(Guid userId, string code, string newPassword, CancellationToken ct = default);
}


