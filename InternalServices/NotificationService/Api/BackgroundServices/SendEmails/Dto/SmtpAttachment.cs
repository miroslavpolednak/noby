namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails.Dto;

public class SmtpAttachment
{
    public string Filename { get; set; } = null!;
    public byte[] Binary { get; set; } = null!;
}
