using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Repositories;
using News.Api.Services;
using News.Api.Utils;
using System.Text;

public class NewsApiService : INewsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _newsApiUrl;
    private readonly string _newsApiKey;
    private readonly List<string> _categories;
    private readonly ICategoryService _categoryService;
    private readonly IArticleService _articleService;
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IEmailSender _emailSender;
    private Dictionary<string, Guid> _categoryIdMap;
    public NewsApiService(HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration, ICategoryService categoryService, IArticleService articleService, IUserSubscriptionRepository subscriptionRepository, IEmailSender emailSender)
    {
        _httpClient = httpClient;
        _newsApiUrl = configuration.GetSection("NEWS_API_ENDPOINT").Value ?? string.Empty;
        _newsApiKey = configuration.GetSection("NEWS_API_KEY").Value ?? string.Empty;
        _categories = Constants.NewsApi.Categories;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("SynergieGlobalTest-Client/1.0");
        _categoryService = categoryService;
        _articleService = articleService;
        _subscriptionRepository = subscriptionRepository;
        _emailSender = emailSender;
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
            Console.WriteLine($"Upserted articles for category {categoryName}");
            await NotifySubscribersAsync(catId);
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

    private async Task NotifySubscribersAsync(Guid categoryId)
    {
        var subscribers = await _subscriptionRepository.GetByCategoryAsync(categoryId, onlyActive: true);
        if (subscribers.Count == 0)
        {
            Console.WriteLine($"No subscribers found for category {categoryId}");
            return;
        }

        var topArticles = await _articleService.GetTop10ArticleByCategoryIdAsync(categoryId);
        if (topArticles.Count == 0)
        {   
            Console.WriteLine($"No top articles found for category {categoryId}");
            return;
        }

        var today = DateTime.UtcNow.Date;
        var todaysArticles = topArticles
            .Where(a => a.PublicationDate.Date == today)
            .OrderByDescending(a => a.PublicationDate)
            .ToList();
        if (todaysArticles.Count == 0)
        {
            Console.WriteLine($"No today articles found for category {categoryId}");
            return;
        }

        var categoryName = _categoryIdMap.FirstOrDefault(kv => kv.Value == categoryId).Key ?? "news";
        var sb = new StringBuilder();
        sb.Append($"<h3>Top {categoryName} news (today)</h3><ol>");
        foreach (var a in todaysArticles)
        {
            sb.Append($"<li><a href='" + a.Url + "'>" + System.Net.WebUtility.HtmlEncode(a.Headline) + "</a> - <i>" + System.Net.WebUtility.HtmlEncode(a.Source) + "</i></li>");
        }
        sb.Append("</ol>");
        var subject = $"Your {categoryName} news digest";

        foreach (var s in subscribers)
        {
            var to = s.User.Email;
            if (string.IsNullOrWhiteSpace(to)) continue;
            Console.WriteLine($"Sending email to {to}");
            await _emailSender.SendAsync(to, subject, sb.ToString());
        }
    }

    private Article convertToArticleEntity(NewsApiArticle article, Guid categoryId)
    {
        return new Article
        {
            Headline = Truncate(article.Title, 200),
            Summary = Truncate(article.Description, 1000),
            Content = article.Content ?? string.Empty,
            PublicationDate = article.PublishedAt ?? DateTime.UtcNow,
            Source = Truncate(article?.Source?.Name, 100),
            Url = Truncate(article.Url, 500),
            CategoryId = categoryId,
        };
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed.Substring(0, maxLength);
    }
}