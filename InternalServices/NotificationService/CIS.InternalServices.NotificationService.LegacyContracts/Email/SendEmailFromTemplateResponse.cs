using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email;

[ProtoContract]
public class SendEmailFromTemplateResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}