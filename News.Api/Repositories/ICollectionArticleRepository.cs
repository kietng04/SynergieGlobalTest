using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface ICollectionArticleRepository
{
    Task<CollectionArticle?> GetAsync(Guid collectionId, Guid articleId);
    Task<CollectionArticle> CreateAsync(CollectionArticle entity);
    Task RemoveArticleAsync(Guid collectionId, Guid articleId);
    Task<List<CollectionArticle>> GetByCollectionAsync(Guid collectionId);
    Task<List<CollectionArticle>> GetByArticleAsync(Guid articleId);

    Task RemoveCollectionAsync(Guid collectionId);
}