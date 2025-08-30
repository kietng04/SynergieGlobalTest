using News.Api.Models.Entities;

namespace News.Api.Services;

public interface IUserSubscriptionService
{
    Task<UserSubscription> AddAsync(Guid userId, Guid categoryId, string emailFrequency, bool isActive);
    Task RemoveAsync(Guid userId, Guid categoryId);
    Task<List<UserSubscription>> GetByUserAsync(Guid userId);
}