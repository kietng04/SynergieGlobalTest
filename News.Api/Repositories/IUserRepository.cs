using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface IUserRepository
{
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
}


