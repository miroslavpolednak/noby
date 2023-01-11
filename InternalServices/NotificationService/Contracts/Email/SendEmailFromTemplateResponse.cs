using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract]
public class SendEmailFromTemplateResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}