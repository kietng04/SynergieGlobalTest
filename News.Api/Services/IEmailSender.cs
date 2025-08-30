namespace News.Api.Services;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string htmlBody, string? from = null, CancellationToken ct = default);
}


