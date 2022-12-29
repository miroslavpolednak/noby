namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public class SmtpAttachment
{
    public string Filename { get; set; }
    public byte[] Binary { get; set; }
}