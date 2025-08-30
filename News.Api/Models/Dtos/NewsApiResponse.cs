using System.Text.Json.Serialization;

namespace News.Api.Models.Dtos;

public class NewsApiResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("totalResults")]
    public int TotalResults { get; set; }

    [JsonPropertyName("articles")]
    public List<NewsApiArticle> Articles { get; set; } = new();
}

public class NewsApiArticle
{
    [JsonPropertyName("source")]
    public NewsApiSource Source { get; set; } = new();

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("urlToImage")]
    public string? UrlToImage { get; set; }

    [JsonPropertyName("publishedAt")]
    public DateTime? PublishedAt { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

public class NewsApiSource
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

 