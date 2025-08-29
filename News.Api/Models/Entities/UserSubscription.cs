namespace News.Api.Models.Entities;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string EmailFrequency { get; set; } = "Daily";
    public bool IsActive { get; set; } = true;
    
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
