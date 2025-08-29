using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Repositories;
using News.Api.Services.Auth;

namespace News.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtService _jwtService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHashingService = passwordHashingService;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username))
        {
            throw new ArgumentException("Username already exists");
        }

        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new ArgumentException("Email already exists");
        }

        var user = new User
        {
            Username = request.Username.Trim(),
            Email = request.Email.Trim(),
            Password = _passwordHashingService.HashPassword(request.Password),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user = await _userRepository.CreateAsync(user);

        var token = _jwtService.GenerateToken(user);

        return new RegisterResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role ?? "User",
            Token = token
        };
    }
}


