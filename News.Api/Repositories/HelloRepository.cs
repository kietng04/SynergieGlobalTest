using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class HelloRepository : IHelloRepository
{
    private readonly ILogger<HelloRepository> _logger;
    
    private static readonly List<HelloMessage> _messages = new();
    private static int _totalRequests = 0;
    private static readonly string _defaultMessage = "Hello World! Welcome to News API 🌍";

    public HelloRepository(ILogger<HelloRepository> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetDefaultMessageAsync()
    {
        _logger.LogDebug("Getting default hello message");
        
        await Task.Delay(10);
        
        Interlocked.Increment(ref _totalRequests);
        
        return _defaultMessage;
    }

    public async Task<string> CreatePersonalizedMessageAsync(string name)
    {
        _logger.LogDebug("Creating personalized message for {Name}", name);
        
        await Task.Delay(10);
        
        var message = $"Hello {name}! Welcome to News API 🎉";
        
        var helloMessage = new HelloMessage
        {
            Id = Guid.NewGuid(),
            Name = name,
            Message = message,
            CreatedAt = DateTime.UtcNow
        };
        
        _messages.Add(helloMessage);
        Interlocked.Increment(ref _totalRequests);
        
        return message;
    }

    public async Task<HelloStats> GetMessageStatsAsync()
    {
        _logger.LogDebug("Getting message statistics");
        
        await Task.Delay(10);
        
        return new HelloStats
        {
            TotalRequests = _totalRequests,
            PersonalizedMessages = _messages.Count,
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<List<HelloMessage>> GetAllMessagesAsync()
    {
        _logger.LogDebug("Getting all hello messages");
        
        await Task.Delay(10);
        
        return _messages.ToList();
    }
}
