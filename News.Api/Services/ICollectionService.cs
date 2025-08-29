using News.Api.Models.Entities;

namespace News.Api.Services;

public interface ICollectionService
{
    Task<Collection> CreateAsync(Guid userId, string name, string description);
    Task<Collection> UpdateAsync(Guid collectionId, Guid userId, string name, string description);
    Task DeleteAsync(Guid collectionId, Guid userId);
    Task<Collection?> GetByIdAsync(Guid collectionId, Guid userId);
    Task<List<Collection>> GetByUserAsync(Guid userId);
    Task<(int articleCount, DateTime? lastUpdated)> GetStatsAsync(Guid collectionId, Guid userId);
    Task<List<Collection>> GetAllCollectionsByUserIdAsync(Guid userId);
}


