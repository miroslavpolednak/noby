using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;

[ProtoContract]
public class EmailAddress
{
    [ProtoMember(1)]
    public string Value { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public Party? Party { get; set; }
}