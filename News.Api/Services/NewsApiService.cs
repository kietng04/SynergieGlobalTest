using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Repositories;
using News.Api.Services;
using News.Api.Utils;

public class NewsApiService : INewsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _newsApiUrl;
    private readonly string _newsApiKey;
    private readonly List<string> _categories;
    private readonly ICategoryService _categoryService;
    private readonly IArticleService _articleService;
    private Dictionary<string, Guid> _categoryIdMap;
    public NewsApiService(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration, ICategoryService categoryService, IArticleService articleService)
    {
        _httpClient = httpClient;
        _newsApiUrl = configuration.GetSection("NEWS_API_ENDPOINT").Value ?? string.Empty;
        _newsApiKey = configuration.GetSection("NEWS_API_KEY").Value ?? string.Empty;
        _categories = Constants.NewsApi.Categories;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SynergieGlobalTest-Client/1.0");
        _categoryService = categoryService;
        _articleService = articleService;
        _categoryIdMap = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        mappingCategoryIdMapAsync().Wait();
    }

    private async Task mappingCategoryIdMapAsync()
    {
        foreach (var name in _categories)
        {
            var id = await _categoryService.GetIdByNameAsync(name);
            _categoryIdMap[name] = id;
        }
    }

    public async Task SyncTopNewsToDatabase()
    {

        foreach (var categoryName in _categories)
        {
            var url = $"{_newsApiUrl}/top-headlines?country=us&apiKey={_newsApiKey}&category={categoryName}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            var newsApiResponse = await response.Content.ReadFromJsonAsync<NewsApiResponse>();
            if (!_categoryIdMap.TryGetValue(categoryName, out var catId))
            {
                continue;
            }
            await handleUpsertArticles(newsApiResponse?.Articles, catId);
        }

    }

    private async Task handleUpsertArticles(List<NewsApiArticle>? articles, Guid categoryId)
    {
        if (articles == null) return;
        foreach (var article in articles)
        {
            var articalEntity = convertToArticleEntity(article, categoryId);
            await _articleService.SyncArticleAsync(articalEntity);
        }
    }

    private Article convertToArticleEntity(NewsApiArticle article, Guid categoryId)
    {
        return new Article
        {
            Headline = article.Title,
            Summary = article.Description ?? string.Empty,
            PublicationDate = article.PublishedAt ?? DateTime.UtcNow,
            Source = article.Source.Name,
            Url = article.Url,
            CategoryId = categoryId,
        };
    }
}