namespace News.Api.Models.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
}