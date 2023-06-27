using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public class SmtpAdapterService : ISmtpAdapterService
{
    private readonly SmtpConfiguration _smtpConfiguration;

    public SmtpAdapterService(IOptions<SmtpConfiguration> options)
    {
        _smtpConfiguration = options.Value;
    }

    public async Task SendEmail(
        string from, string replyTo, string subject, string content,
        IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
        IEnumerable<SmtpAttachment> attachments)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.SecureSocket);

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));

        if (!string.IsNullOrEmpty(replyTo))
        {
            message.ReplyTo.Add(MailboxAddress.Parse(replyTo));
        }
        
        message.Subject = subject;
        
        foreach (var t in to)
        {
            if (!string.IsNullOrEmpty(t))
            {
                message.To.Add(MailboxAddress.Parse(t));
            }
        }
        
        foreach (var c in cc)
        {
            if (!string.IsNullOrEmpty(c))
            {
                message.Cc.Add(MailboxAddress.Parse(c));
            }
        }
        
        foreach (var b in bcc)
        {
            if (!string.IsNullOrEmpty(b))
            {
                message.Bcc.Add(MailboxAddress.Parse(b));
            }
        }

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody =  content;

        foreach (var attachment in attachments)
        {
            bodyBuilder.Attachments.Add(attachment.Filename, attachment.Binary);
        }

        message.Body = bodyBuilder.ToMessageBody();

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}