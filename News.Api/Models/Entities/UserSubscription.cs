namespace News.Api.Models.Entities;

public class UserSubscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string EmailFrequency { get; set; } = "Weekly"; // Daily, Weekly
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
