using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Net.Security;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public class SmtpAdapterService : ISmtpAdapterService
{
    private readonly SmtpConfiguration _smtpConfiguration;
    private readonly ILogger<SmtpAdapterService> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private const int _maxRetries = 3;

    public SmtpAdapterService(
        IOptions<SmtpConfiguration> smtpOptions,
        ILogger<SmtpAdapterService> logger)
    {
        _smtpConfiguration = smtpOptions.Value;
        _logger = logger;
        _retryPolicy = CreatePolicy();
    }

    public async Task SendEmail(
        string format,
        string from, string replyTo, string subject, string content,
        IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
        IEnumerable<SmtpAttachment> attachments)
    {
        using var client = new SmtpClient()
        {
            CheckCertificateRevocation = false,
            Timeout = _smtpConfiguration.Timeout * 1000,
            ServerCertificateValidationCallback = (sender, certificate, chain, errors) => {
                if (errors == SslPolicyErrors.None)
                    return true;

                return _smtpConfiguration.DisableServerCertificateValidation;
            }
        };

        await _retryPolicy.ExecuteAsync(async () =>
               await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.SecureSocket)
            );

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

    private AsyncRetryPolicy CreatePolicy()
    {
        return Policy.Handle<MailKit.Security.SslHandshakeException>().WaitAndRetryAsync(_maxRetries, retryAttempt => TimeSpan.FromSeconds(1), onRetry: (exp, interval, retryCount, context) =>
        {
            _logger.LogWarning(exp, $"SmtpClient RetryPolicy handle exception, retry count {retryCount}");
        });
    }
}