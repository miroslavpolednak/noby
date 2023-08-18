using MailKit.Security;

namespace Console_ClientSMTP;

public class SmtpConfiguration
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public SecureSocketOptions SecureSocketOptions { get; set; }    
}