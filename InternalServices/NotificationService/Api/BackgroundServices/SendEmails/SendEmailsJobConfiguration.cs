using MailKit.Security;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

public sealed class SendEmailsJobConfiguration
{
    public int EmailSlaInMinutes { get; set; }

    public int NumberOfEmailsAtOnce { get; set; }

    public required SmtpConfiguration SmtpConfiguration { get; set; }
}

public class SmtpConfiguration
{
    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public SecureSocketOptions SecureSocket { get; set; } = SecureSocketOptions.None;

    public int Timeout { get; set; } = 60;

    public bool DisableServerCertificateValidation { get; set; }
}