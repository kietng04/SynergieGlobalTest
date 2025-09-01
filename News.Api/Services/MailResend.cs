using System.Net.Mail;
using Microsoft.Extensions.Options;
using News.Api.Services;
using Resend;

namespace News.Api.Services;

public class MailResend : IEmailSender
{
    private readonly IResend _resend;

    public MailResend(IResend resend)
    {   
        _resend = resend;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        var message = new EmailMessage();
        message.From = "Acme <onboarding@resend.dev>";
        message.To.Add(to);
        message.Subject = subject;
        message.HtmlBody = htmlBody;
        await _resend.EmailSendAsync( message );
    }


}


