using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly NewsDbContext _dbContext;

    public CollectionRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Collection> CreateAsync(Collection collection)
    {
        _dbContext.Collections.Add(collection);
        await _dbContext.SaveChangesAsync();
        return collection;
    }

    public Task<Collection?> GetByIdAsync(Guid id)
    {
        return _dbContext.Collections.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Collection>> GetByUserAsync(Guid userId)
    {
        return _dbContext.Collections
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(Collection collection)
    {
        _dbContext.Collections.Update(collection);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Collection collection)
    {
        _dbContext.Collections.Remove(collection);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> NameExistsForUserAsync(Guid userId, string name, Guid? excludeId = null)
    {
        var query = _dbContext.Collections.AsQueryable()
            .Where(c => c.UserId == userId && c.Name == name);
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }
        return query.AnyAsync();
    }

    public Task<List<Collection>> GetAllCollectionsByUserIdAsync(Guid userId)
    {
        return _dbContext.Collections
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }
}