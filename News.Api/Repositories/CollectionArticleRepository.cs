using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class CollectionArticleRepository : ICollectionArticleRepository
{
    private readonly NewsDbContext _dbContext;

    public CollectionArticleRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CollectionArticle?> GetAsync(Guid collectionId, Guid articleId)
    {
        return _dbContext.CollectionArticles
            .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.ArticleId == articleId);
    }

    public async Task<CollectionArticle> CreateAsync(CollectionArticle entity)
    {
        _dbContext.CollectionArticles.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task RemoveAsync(CollectionArticle entity)
    {
        _dbContext.CollectionArticles.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<CollectionArticle>> GetByCollectionAsync(Guid collectionId)
    {
        return _dbContext.CollectionArticles
            .Where(x => x.CollectionId == collectionId)
            .OrderByDescending(x => x.SavedAt)
            .ToListAsync();
    }

    public Task<List<CollectionArticle>> GetByArticleAsync(Guid articleId)
    {
        return _dbContext.CollectionArticles
            .Where(x => x.ArticleId == articleId)
            .ToListAsync();
    }
}