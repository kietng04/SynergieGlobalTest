using System.ComponentModel.DataAnnotations;

namespace News.Api.Models.Dtos;

public class CreateCollectionRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class CollectionResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CollectionArticleResponseDto
{
    public Guid Id { get; set; }
    public Guid CollectionId { get; set; }
    public Guid ArticleId { get; set; }
    public DateTime SavedAt { get; set; }
}

public class UpdateCollectionRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}


