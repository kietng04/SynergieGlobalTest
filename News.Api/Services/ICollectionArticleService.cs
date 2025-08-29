using News.Api.Models.Entities;

namespace News.Api.Services;

public interface ICollectionArticleService
{
    Task<CollectionArticle> AddAsync(Guid collectionId, Guid articleId, Guid userId);
    Task RemoveCollectionAsync(Guid collectionId, Guid userId);
    Task RemoveArticleAsync(Guid collectionId, Guid articleId, Guid userId);
}


