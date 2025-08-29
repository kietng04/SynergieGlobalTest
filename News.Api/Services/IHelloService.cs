using News.Api.Models.Dtos;

namespace News.Api.Services;

public interface IHelloService
{
    Task<HelloResponseDto> GetHelloMessageAsync();
    Task<HelloResponseDto> GetPersonalizedHelloAsync(string name);
    Task<HelloStatsDto> GetHelloStatsAsync();
}
