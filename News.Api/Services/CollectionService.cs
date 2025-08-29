using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;
using News.Api.Repositories;

namespace News.Api.Services;

public class CollectionService : ICollectionService
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly NewsDbContext _dbContext;
    private readonly ILogger<CollectionService> _logger;

    public CollectionService(
        ICollectionRepository collectionRepository,
        NewsDbContext dbContext,
        ILogger<CollectionService> logger)
    {
        _collectionRepository = collectionRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Collection> CreateAsync(Guid userId, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Collection name is required");
        }

        name = name.Trim();
        description = description?.Trim() ?? string.Empty;

        if (await _collectionRepository.NameExistsForUserAsync(userId, name))
        {
            throw new ArgumentException("Collection name already exists for this user");
        }

        var collection = new Collection
        {
            UserId = userId,
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _collectionRepository.CreateAsync(collection);
        _logger.LogInformation("Collection created {CollectionId} for user {UserId}", created.Id, userId);
        return created;
    }

    public async Task<Collection> UpdateAsync(Guid collectionId, Guid userId, string name, string description)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId)
            ?? throw new KeyNotFoundException("Collection not found");

        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Collection name is required");
        }

        name = name.Trim();
        description = description?.Trim() ?? string.Empty;

        if (await _collectionRepository.NameExistsForUserAsync(userId, name, collectionId))
        {
            throw new ArgumentException("Collection name already exists for this user");
        }

        collection.Name = name;
        collection.Description = description;
        collection.UpdatedAt = DateTime.UtcNow;

        await _collectionRepository.UpdateAsync(collection);
        return collection;
    }

    public async Task DeleteAsync(Guid collectionId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId)
            ?? throw new KeyNotFoundException("Collection not found");

        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }

        await _collectionRepository.DeleteAsync(collection);
        _logger.LogInformation("Collection deleted {CollectionId} by user {UserId}", collectionId, userId);
    }

    public async Task<Collection?> GetByIdAsync(Guid collectionId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId);
        if (collection == null || collection.UserId != userId)
        {
            return null;
        }
        return collection;
    }

    public Task<List<Collection>> GetByUserAsync(Guid userId)
    {
        return _collectionRepository.GetByUserAsync(userId);
    }

    public async Task<(int articleCount, DateTime? lastUpdated)> GetStatsAsync(Guid collectionId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId)
            ?? throw new KeyNotFoundException("Collection not found");

        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }

        var articles = await _dbContext.CollectionArticles
            .Where(x => x.CollectionId == collectionId)
            .ToListAsync();

        var count = articles.Count;
        DateTime? lastUpdated = articles.Count == 0 ? null : articles.Max(x => x.SavedAt);
        return (count, lastUpdated);
    }
}


