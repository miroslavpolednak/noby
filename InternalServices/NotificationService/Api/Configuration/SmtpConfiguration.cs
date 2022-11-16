using MailKit.Security;

namespace CIS.InternalServices.NotificationService.Api.Configuration;

public class SmtpConfiguration
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public SecureSocketOptions SecureSocket { get; set; } = SecureSocketOptions.None;
}