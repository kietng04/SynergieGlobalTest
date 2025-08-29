using System.ComponentModel.DataAnnotations;

namespace News.Api.Models.Dtos;

public class HelloRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public string Name { get; set; } = string.Empty;
}

public class HelloResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string? Recipient { get; set; }
    public string Version { get; set; } = string.Empty;
    public List<string> Features { get; set; } = new();
    public DateTime Timestamp { get; set; }
}

public class HelloStatsDto
{
    public int TotalRequests { get; set; }
    public int PersonalizedMessages { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; set; }
}
