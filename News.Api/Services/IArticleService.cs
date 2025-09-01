using News.Api.Models.Entities;

namespace News.Api.Services;

public interface IArticleService
{
    Task<Article> SyncArticleAsync(Article article);
    Task<List<Article>> GetTop10ArticleByCategoryIdAsync(Guid categoryId);
}


