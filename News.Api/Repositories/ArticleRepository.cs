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
}


