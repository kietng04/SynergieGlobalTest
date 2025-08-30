using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;
using News.Api.Repositories;

namespace News.Api.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly NewsDbContext _dbContext;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(
        IArticleRepository articleRepository,
        NewsDbContext dbContext,
        ILogger<ArticleService> logger)
    {
        _articleRepository = articleRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task<Article> SyncArticleAsync(Article article)
    {
        return _articleRepository.SyncArticleAsync(article);
    }

}


