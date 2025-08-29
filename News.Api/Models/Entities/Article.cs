namespace News.Api.Models.Entities;

public class Article : BaseEntity
{
    public string Headline { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    
    public Category Category { get; set; } = null!;
    public ICollection<CollectionArticle> CollectionArticles { get; set; } = new List<CollectionArticle>();
}