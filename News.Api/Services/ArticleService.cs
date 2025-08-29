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

    public async Task<Article> AddArticleAsync(Article article)
    {
        if (article == null)
        {
            throw new ArgumentNullException(nameof(article));
        }

        if (string.IsNullOrWhiteSpace(article.Url))
        {
            throw new ArgumentException("Article URL is required");
        }

        // Normalize input strings
        article.Headline = article.Headline?.Trim() ?? string.Empty;
        article.Summary = article.Summary?.Trim() ?? string.Empty;
        article.Source = article.Source?.Trim() ?? string.Empty;
        article.Url = article.Url.Trim();

        // If an article with the same URL already exists, return it (idempotent add)
        var existing = await _articleRepository.GetByUrlAsync(article.Url);
        if (existing != null)
        {
            return existing;
        }

        // Validate category existence
        var categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == article.CategoryId);
        if (!categoryExists)
        {
            throw new ArgumentException("Category does not exist");
        }

        article.PublicationDate = article.PublicationDate == default
            ? DateTime.UtcNow
            : article.PublicationDate;
        article.UpdatedAt = DateTime.UtcNow;

        var created = await _articleRepository.CreateAsync(article);
        _logger.LogInformation("Article added with Id {ArticleId} and Url {Url}", created.Id, created.Url);
        return created;
    }
}


