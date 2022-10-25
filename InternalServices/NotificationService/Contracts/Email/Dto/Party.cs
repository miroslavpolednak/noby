using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email.Dto;

[ProtoContract]
public class Party
{
    [ProtoMember(2)]
    public LegalPerson? LegalPerson { get; set; }
    
    [ProtoMember(3)]
    public NaturalPerson? NaturalPerson { get; set; } 
}