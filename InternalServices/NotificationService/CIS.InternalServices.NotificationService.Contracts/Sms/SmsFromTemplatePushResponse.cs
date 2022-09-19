using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SmsFromTemplatePushResponse
{
    [ProtoMember(1)]
    public string NotificationId { get; set; } = string.Empty;
}