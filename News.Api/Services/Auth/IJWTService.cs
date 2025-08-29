using System.Security.Claims;
using News.Api.Models.Entities;

namespace News.Api.Services.Auth;

public interface IJwtService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}