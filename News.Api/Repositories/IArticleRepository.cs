using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface IArticleRepository
{
    Task<Article?> GetByUrlAsync(string url);
    Task<Article> CreateAsync(Article article);
    Task<Article> SyncArticleAsync(Article article);
    Task<List<Article>> GetTop10ArticleByCategoryIdAsync(Guid categoryId);
}


