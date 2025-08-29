using News.Api.Models.Entities;

namespace News.Api.Services;

public interface IArticleService
{
    Task<Article> AddArticleAsync(Article article);
}


