namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Email.Models;

public class SendEmailAttachment
{
    public string S3Key { get; set; } = null!;
    public string Filename { get; set; } = null!;
}