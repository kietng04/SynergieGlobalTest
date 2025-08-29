namespace News.Api.Models.Entities;

public class Collection : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<CollectionArticle> CollectionArticles { get; set; } = new List<CollectionArticle>();
}
