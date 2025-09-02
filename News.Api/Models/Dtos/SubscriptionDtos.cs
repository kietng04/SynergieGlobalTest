using System.ComponentModel.DataAnnotations;

namespace News.Api.Models.Dtos;

public class CreateSubscriptionRequestDto
{
    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [StringLength(20)]
    public string EmailFrequency { get; set; } = "Daily";

    public bool IsActive { get; set; } = true;
}

public class SubscriptionResponseDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string EmailFrequency { get; set; } = "Daily";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UpdateSubscriptionRequestDto
{
    [StringLength(20)]
    public string? EmailFrequency { get; set; }
    public bool? IsActive { get; set; }
}