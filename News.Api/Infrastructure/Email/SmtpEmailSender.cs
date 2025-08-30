using System.Net.Mail;
using Microsoft.Extensions.Options;
using News.Api.Services;

namespace News.Api.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly SmtpSettings _settings;

    public SmtpEmailSender(SmtpClient smtpClient, IOptions<SmtpSettings> options)
    {
        _smtpClient = smtpClient;
        _settings = options.Value;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, string? from = null, CancellationToken ct = default)
    {
        var configuredFrom = string.IsNullOrWhiteSpace(_settings.From) ? null : _settings.From;
        var fromAddress = string.IsNullOrWhiteSpace(from) ? (configuredFrom ?? "no-reply@example.com") : from;
        var message = new MailMessage(fromAddress, to, subject, htmlBody)
        {
            IsBodyHtml = true
        };
        await _smtpClient.SendMailAsync(message);
    }
}


