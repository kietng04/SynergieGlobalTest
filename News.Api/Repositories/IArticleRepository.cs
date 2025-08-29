using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface IArticleRepository
{
    Task<Article?> GetByUrlAsync(string url);
    Task<Article> CreateAsync(Article article);
}


