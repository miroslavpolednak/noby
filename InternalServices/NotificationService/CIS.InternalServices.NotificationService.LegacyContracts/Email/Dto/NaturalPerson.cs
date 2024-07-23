using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;

[ProtoContract]
public class NaturalPerson
{
    [ProtoMember(1)]
    public string FirstName { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string MiddleName { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public string Surname { get; set; } = string.Empty;
}