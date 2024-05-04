namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails.Dto;

internal sealed class SmtpAttachment
{
    public string Filename { get; set; } = null!;
    public byte[] Binary { get; set; } = null!;
}
