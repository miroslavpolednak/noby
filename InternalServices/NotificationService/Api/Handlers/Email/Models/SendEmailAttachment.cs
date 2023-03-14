namespace CIS.InternalServices.NotificationService.Api.Handlers.Email.Models;

public class SendEmailAttachment
{
    public string S3Key { get; set; } = null!;
    public string Filename { get; set; } = null!;
}