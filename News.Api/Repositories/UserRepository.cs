using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NewsDbContext _dbContext;

    public UserRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> UsernameExistsAsync(string username)
    {
        return _dbContext.Users.AnyAsync(u => u.Username == username);
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return _dbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}


