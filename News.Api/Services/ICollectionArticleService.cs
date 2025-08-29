using News.Api.Models.Entities;

namespace News.Api.Services;

public interface ICollectionArticleService
{
    Task<CollectionArticle> AddAsync(Guid collectionId, Guid articleId, Guid userId);
}


