using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;

[ProtoContract]
public class Party
{
    [ProtoMember(2)]
    public LegalPerson? LegalPerson { get; set; }
    
    [ProtoMember(3)]
    public NaturalPerson? NaturalPerson { get; set; } 
}