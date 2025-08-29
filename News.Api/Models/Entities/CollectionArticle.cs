namespace News.Api.Models.Entities;

public class CollectionArticle : BaseEntity
{
    public Guid CollectionId { get; set; }
    public Guid ArticleId { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Collection Collection { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
