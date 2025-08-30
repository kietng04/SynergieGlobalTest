using News.Api.Models.Entities;

namespace News.Api.Services;

public interface IArticleService
{
    Task<Article> SyncArticleAsync(Article article);
}


