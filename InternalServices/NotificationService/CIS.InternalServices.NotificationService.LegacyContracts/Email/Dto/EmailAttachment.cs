using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;

[ProtoContract]
public class EmailAttachment
{
    [ProtoMember(1)]
    public string Binary { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string Filename { get; set; } = string.Empty;
}