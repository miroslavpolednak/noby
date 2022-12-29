namespace CIS.InternalServices.NotificationService.Api.Handlers.Email.Models;

public class SendEmailAttachment
{
    public string S3Key { get; set; }
    public string Filename { get; set; }
}