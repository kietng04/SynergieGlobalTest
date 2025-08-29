using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface IHelloRepository
{
    Task<string> GetDefaultMessageAsync();
    Task<string> CreatePersonalizedMessageAsync(string name);
    Task<HelloStats> GetMessageStatsAsync();
    Task<List<HelloMessage>> GetAllMessagesAsync();
}
