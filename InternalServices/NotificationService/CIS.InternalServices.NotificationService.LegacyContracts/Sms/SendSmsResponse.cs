using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Sms;

[ProtoContract]
public class SendSmsResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}