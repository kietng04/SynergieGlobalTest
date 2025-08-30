using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class UserSubscriptionRepository : IUserSubscriptionRepository
{
    private readonly NewsDbContext _dbContext;

    public UserSubscriptionRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserSubscription?> GetAsync(Guid userId, Guid categoryId)
    {
        return _dbContext.UserSubscriptions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CategoryId == categoryId);
    }

    public Task<List<UserSubscription>> GetByUserAsync(Guid userId)
    {
        return _dbContext.UserSubscriptions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();
    }

    public async Task<UserSubscription> CreateAsync(UserSubscription entity)
    {
        _dbContext.UserSubscriptions.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(UserSubscription entity)
    {
        _dbContext.UserSubscriptions.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId, Guid categoryId)
    {
        var sub = await _dbContext.UserSubscriptions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CategoryId == categoryId);
        if (sub == null)
        {
            throw new KeyNotFoundException("Subscription not found");
        }
        _dbContext.UserSubscriptions.Remove(sub);
        await _dbContext.SaveChangesAsync();
    }
}