using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface ICollectionRepository
{
    Task<Collection> CreateAsync(Collection collection);
    Task<Collection?> GetByIdAsync(Guid id);
    Task<List<Collection>> GetByUserAsync(Guid userId);
    Task UpdateAsync(Collection collection);
    Task DeleteAsync(Collection collection);
    Task<bool> NameExistsForUserAsync(Guid userId, string name, Guid? excludeId = null);
}