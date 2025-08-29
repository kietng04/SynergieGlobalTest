using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface ICollectionArticleRepository
{
    Task<CollectionArticle?> GetAsync(Guid collectionId, Guid articleId);
    Task<CollectionArticle> CreateAsync(CollectionArticle entity);
    Task RemoveAsync(CollectionArticle entity);
    Task<List<CollectionArticle>> GetByCollectionAsync(Guid collectionId);
    Task<List<CollectionArticle>> GetByArticleAsync(Guid articleId);
}