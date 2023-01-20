using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SendSmsResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}