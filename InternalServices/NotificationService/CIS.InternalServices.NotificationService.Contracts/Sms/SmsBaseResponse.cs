using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
[ProtoInclude(1, typeof(SmsPushResponse))]
[ProtoInclude(2, typeof(SmsFromTemplatePushResponse))]
public class SmsBaseResponse
{
    [ProtoMember(1)]
    public string NotificationId { get; set; } = string.Empty;
}