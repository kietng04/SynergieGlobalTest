using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;
using News.Api.Repositories;

namespace News.Api.Services;

public class UserSubscriptionService : IUserSubscriptionService
{
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly NewsDbContext _dbContext;
    private readonly ILogger<UserSubscriptionService> _logger;

    public UserSubscriptionService(
        IUserSubscriptionRepository subscriptionRepository,
        NewsDbContext dbContext,
        ILogger<UserSubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<UserSubscription> AddAsync(Guid userId, Guid categoryId, string emailFrequency, bool isActive)
    {
        var categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        if (!categoryExists)
        {
            throw new ArgumentException("Category does not exist");
        }

        var existing = await _subscriptionRepository.GetAsync(userId, categoryId);
        if (existing != null)
        {
            throw new ArgumentException("Subscription already exists for this category");
        }

        var entity = new UserSubscription
        {
            UserId = userId,
            CategoryId = categoryId,
            EmailFrequency = string.IsNullOrWhiteSpace(emailFrequency) ? "Daily" : emailFrequency.Trim(),
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _subscriptionRepository.CreateAsync(entity);
        _logger.LogInformation("User {UserId} subscribed to category {CategoryId}", userId, categoryId);
        return created;
    }

    public async Task RemoveAsync(Guid userId, Guid categoryId)
    {
        await _subscriptionRepository.DeleteAsync(userId, categoryId);
        _logger.LogInformation("User {UserId} unsubscribed category {CategoryId}", userId, categoryId);
    }

    public Task<List<UserSubscription>> GetByUserAsync(Guid userId)
    {
        return _subscriptionRepository.GetByUserAsync(userId);
    }

    public async Task<UserSubscription> UpdateAsync(Guid userId, Guid categoryId, string? emailFrequency, bool? isActive)
    {
        var sub = await _subscriptionRepository.GetAsync(userId, categoryId)
            ?? throw new KeyNotFoundException("Subscription not found");

        if (!string.IsNullOrWhiteSpace(emailFrequency))
        {
            sub.EmailFrequency = emailFrequency.Trim();
        }
        if (isActive.HasValue)
        {
            sub.IsActive = isActive.Value;
        }
        sub.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        return sub;
    }
}