using CIS.Infrastructure.Attributes;
using CIS.InternalServices.NotificationService.Api.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

[ScopedService, SelfService]
public class SmtpAdapterService
{
    private readonly SmtpConfiguration _smtpConfiguration;

    public SmtpAdapterService(IOptions<SmtpConfiguration> options)
    {
        _smtpConfiguration = options.Value;
    }

    public async Task SendEmail(
        string from, string replyTo, string subject, string content,
        IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
        IEnumerable<KeyValuePair<string, byte[]>> attachments)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.SecureSocket);

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));
        message.ReplyTo.Add(MailboxAddress.Parse(replyTo));
        message.Subject = subject;
        
        foreach (var t in to)
        {
            message.To.Add(MailboxAddress.Parse(t));
        }
        
        foreach (var c in cc)
        {
            message.To.Add(MailboxAddress.Parse(c));
        }
        
        foreach (var b in bcc)
        {
            message.To.Add(MailboxAddress.Parse(b));
        }

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody =  content;

        foreach (var attachment in attachments)
        {
            bodyBuilder.Attachments.Add(attachment.Key, attachment.Value);
        }

        message.Body = bodyBuilder.ToMessageBody();

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}