using News.Api.Models.Dtos;

namespace News.Api.Services;

public interface IUserService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}


