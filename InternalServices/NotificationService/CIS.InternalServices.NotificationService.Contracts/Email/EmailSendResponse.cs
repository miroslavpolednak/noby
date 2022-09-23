using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class EmailSendResponse
{
    [ProtoMember(1)]
    public string NotificationId { get; set; } = string.Empty;
}