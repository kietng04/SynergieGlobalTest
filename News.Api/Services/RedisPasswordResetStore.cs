using StackExchange.Redis;

namespace News.Api.Services.PasswordReset;

public class RedisPasswordResetStore : IPasswordResetStore
{
    private readonly IConnectionMultiplexer _redis;
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(15);

    public RedisPasswordResetStore(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void Set(Guid userId, string code, TimeSpan? ttl = null, CancellationToken ct = default)
    {
        var db = _redis.GetDatabase();
        db.StringSet(BuildKey(userId), code, ttl ?? DefaultTtl);
    }

    public string? Get(Guid userId, CancellationToken ct = default)
    {
        var db = _redis.GetDatabase();
        var value = db.StringGet(BuildKey(userId));
        return value.HasValue ? value.ToString() : null;
    }

    public void Remove(Guid userId, CancellationToken ct = default)
    {
        var db = _redis.GetDatabase();
        db.KeyDelete(BuildKey(userId));
        return;
    }

    private static string BuildKey(Guid userId) => $"pwdreset:{userId}";
}


