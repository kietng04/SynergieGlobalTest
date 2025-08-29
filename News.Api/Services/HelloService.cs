using News.Api.Models.Dtos;
using News.Api.Repositories;

namespace News.Api.Services;

public class HelloService : IHelloService
{
    private readonly IHelloRepository _helloRepository;
    private readonly ILogger<HelloService> _logger;

    public HelloService(IHelloRepository helloRepository, ILogger<HelloService> logger)
    {
        _helloRepository = helloRepository;
        _logger = logger;
    }

    public async Task<HelloResponseDto> GetHelloMessageAsync()
    {
        _logger.LogInformation("Processing hello message request");
        
        var message = await _helloRepository.GetDefaultMessageAsync();
        
        return new HelloResponseDto
        {
            Message = message,
            Version = "1.0.0",
            Features = new List<string> { "ASP.NET Core", "Layered Architecture", "Web API", "Swagger" },
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<HelloResponseDto> GetPersonalizedHelloAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty or null", nameof(name));
        }

        if (name.Length < 2)
        {
            throw new ArgumentException("Name must be at least 2 characters long", nameof(name));
        }

        if (name.Length > 50)
        {
            throw new ArgumentException("Name cannot exceed 50 characters", nameof(name));
        }

        _logger.LogInformation("Processing personalized hello message for {Name}", name);
        
        var message = await _helloRepository.CreatePersonalizedMessageAsync(name.Trim());
        
        return new HelloResponseDto
        {
            Message = message,
            Recipient = name.Trim(),
            Version = "1.0.0",
            Features = new List<string> { "Personalized Greeting", "Input Validation" },
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<HelloStatsDto> GetHelloStatsAsync()
    {
        _logger.LogInformation("Getting hello statistics");
        
        var stats = await _helloRepository.GetMessageStatsAsync();
        
        return new HelloStatsDto
        {
            TotalRequests = stats.TotalRequests,
            PersonalizedMessages = stats.PersonalizedMessages,
            LastUpdated = stats.LastUpdated
        };
    }
}
