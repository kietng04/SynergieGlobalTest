using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface IUserSubscriptionRepository
{
    Task<UserSubscription?> GetAsync(Guid userId, Guid categoryId);
    Task<List<UserSubscription>> GetByUserAsync(Guid userId);
    Task<List<UserSubscription>> GetByCategoryAsync(Guid categoryId, bool onlyActive = true);
    Task<UserSubscription> CreateAsync(UserSubscription entity);
    Task DeleteAsync(UserSubscription entity);
    Task DeleteAsync(Guid userId, Guid categoryId);
}