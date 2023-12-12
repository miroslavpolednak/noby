using MailKit.Security;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

public sealed class SendEmailsJobConfiguration
{
    public int NumberOfEmailsAtOnce { get; set; }

    public HashSet<string> EmailDomainWhitelist { get; set; } = new();

    public SmtpConfiguration SmtpConfiguration { get; set; } = default!;
}

public class SmtpConfiguration
{
    public string Host { get; set; } = default!;

    public int Port { get; set; }

    public SecureSocketOptions SecureSocket { get; set; } = SecureSocketOptions.None;

    public int Timeout { get; set; } = 60;

    public bool DisableServerCertificateValidation { get; set; }
}