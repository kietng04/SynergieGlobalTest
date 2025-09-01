public class ArticleResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

}