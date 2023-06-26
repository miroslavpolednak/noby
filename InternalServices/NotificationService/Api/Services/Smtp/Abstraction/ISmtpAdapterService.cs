namespace CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;

public interface ISmtpAdapterService
{
    public Task SendEmail(
        string from, string replyTo, string subject, string content,
        IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
        IEnumerable<SmtpAttachment> attachments);
}