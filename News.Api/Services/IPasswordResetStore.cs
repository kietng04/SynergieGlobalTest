namespace News.Api.Services.PasswordReset;

public interface IPasswordResetStore
{
    void Set(Guid userId, string code, TimeSpan? ttl = null, CancellationToken ct = default);
    string? Get(Guid userId, CancellationToken ct = default);
    void Remove(Guid userId, CancellationToken ct = default);
}


