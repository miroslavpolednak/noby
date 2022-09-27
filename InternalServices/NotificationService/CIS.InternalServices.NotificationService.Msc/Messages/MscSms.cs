using CIS.InternalServices.NotificationService.Msc.Messages.Dto;

namespace CIS.InternalServices.NotificationService.Msc.Messages;

public class MscSms
{
    public string NotificationId { get; set; } = string.Empty;
    public Phone Phone { get; set; } = new();
    public string Text { get; set; } = string.Empty;
    public int ProcessPriority { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
    public SmsNotificationType Type { get; set; }
}