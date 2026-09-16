using Application.Options;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Services;

public sealed class EmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _options = options.Value;
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var body = new TextPart(MimeKit.Text.TextFormat.Html);
        body.Text = htmlMessage;
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress(null, "noreply@sportly.com"));
        message.To.Add(new MailboxAddress(null, email));
        message.Subject = subject;
        message.Body = body;
        using var client = new SmtpClient();
        await client.ConnectAsync(options.Value.Host, options.Value.Port);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}