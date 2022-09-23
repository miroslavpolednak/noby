using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email.Dto;

[ProtoContract]
public class EmailContent
{
    [ProtoMember(1)]
    public string Format { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string Language { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public string Text { get; set; } = string.Empty;
}