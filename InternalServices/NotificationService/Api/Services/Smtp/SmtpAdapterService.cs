using System.Globalization;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public class SmtpAdapterService : ISmtpAdapterService
{

    private readonly SmtpConfiguration _smtpConfiguration;
    
    public SmtpAdapterService(IOptions<SmtpConfiguration> smtpOptions)
    {
        _smtpConfiguration = smtpOptions.Value;
    }

    public async Task SendEmail(
        string format,
        string from, string replyTo, string subject, string content,
        IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
        IEnumerable<SmtpAttachment> attachments)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.SecureSocket);

        var message = MimeMessageExtensions
                .Create()
                .AddFrom(from)
                .AddReplyTo(replyTo)
                .AddSubject(subject)
                .AddTo(to)
                .AddCc(cc)
                .AddBcc(bcc)
                .AddContent(format, content, attachments);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}