using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;
using News.Api.Repositories;

namespace News.Api.Services;

public class CollectionArticleService : ICollectionArticleService
{
    private readonly ICollectionArticleRepository _collectionArticleRepository;
    private readonly ICollectionRepository _collectionRepository;
    private readonly NewsDbContext _dbContext;
    private readonly ILogger<CollectionArticleService> _logger;

    public CollectionArticleService(
        ICollectionArticleRepository collectionArticleRepository,
        ICollectionRepository collectionRepository,
        NewsDbContext dbContext,
        ILogger<CollectionArticleService> logger)
    {
        _collectionArticleRepository = collectionArticleRepository;
        _collectionRepository = collectionRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CollectionArticle> AddAsync(Guid collectionId, Guid articleId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId)
            ?? throw new KeyNotFoundException("Collection not found");

        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }

        var articleExists = await _dbContext.Articles.AnyAsync(a => a.Id == articleId);
        if (!articleExists)
        {
            throw new KeyNotFoundException("Article not found");
        }

        var existing = await _collectionArticleRepository.GetAsync(collectionId, articleId);
        if (existing != null)
        {
            throw new ArgumentException("Article already exists in this collection");
        }

        var entity = new CollectionArticle
        {
            CollectionId = collectionId,
            ArticleId = articleId,
            SavedAt = DateTime.UtcNow
        };

        var created = await _collectionArticleRepository.CreateAsync(entity);

        collection.UpdatedAt = DateTime.UtcNow;
        await _collectionRepository.UpdateAsync(collection);

        _logger.LogInformation("Added Article {ArticleId} to Collection {CollectionId} by User {UserId}", articleId, collectionId, userId);
        return created;
    }

    public async Task RemoveArticleAsync(Guid collectionId, Guid articleId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId);
        if (collection == null)
        {
            throw new KeyNotFoundException("Collection not found");
        }
        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }
        await _collectionArticleRepository.RemoveArticleAsync(collectionId, articleId);
    }

    public async Task RemoveCollectionAsync(Guid collectionId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId);
        if (collection == null)
        {
            throw new KeyNotFoundException("Collection not found");
        }
        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }
        await _collectionArticleRepository.RemoveCollectionAsync(collectionId);
        _logger.LogInformation("Removed Collection {CollectionId} by User {UserId}", collectionId, userId);
    }

    public async Task<List<Article>> GetArticlesByCollectionAsync(Guid collectionId, Guid userId)
    {
        var collection = await _collectionRepository.GetByIdAsync(collectionId)
            ?? throw new KeyNotFoundException("Collection not found");
        if (collection.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not own this collection");
        }

        var articles = await _dbContext.CollectionArticles
            .Where(x => x.CollectionId == collectionId)
            .OrderByDescending(x => x.SavedAt)
            .Select(x => x.Article)
            .ToListAsync();
        return articles;
    }
}


