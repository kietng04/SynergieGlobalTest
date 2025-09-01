using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly NewsDbContext _dbContext;

    public ArticleRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Article?> GetByUrlAsync(string url)
    {
        return _dbContext.Articles.FirstOrDefaultAsync(a => a.Url == url);
    }

    public async Task<Article> CreateAsync(Article article)
    {
        _dbContext.Articles.Add(article);
        await _dbContext.SaveChangesAsync();
        return article;
    }

    public async Task<Article> SyncArticleAsync(Article article)
    {
        var existing = await GetByUrlAsync(article.Url);
        if (existing == null)
        {
            return await CreateAsync(article);
        }

        existing.Headline = article.Headline;
        existing.Summary = article.Summary;
        existing.Content = article.Content;
        existing.PublicationDate = article.PublicationDate;
        existing.Source = article.Source;
        existing.CategoryId = article.CategoryId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<List<Article>> GetTop10ArticleByCategoryIdAsync(Guid categoryId)
    {
        return await _dbContext.Articles.Where(a => a.CategoryId == categoryId).OrderByDescending(a => a.PublicationDate).Take(10).ToListAsync();
    }
}


