using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email;

[ProtoContract]
public class SendEmailResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}