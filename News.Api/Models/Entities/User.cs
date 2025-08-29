namespace News.Api.Models.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Role { get; set; }
    
    public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    public ICollection<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
}