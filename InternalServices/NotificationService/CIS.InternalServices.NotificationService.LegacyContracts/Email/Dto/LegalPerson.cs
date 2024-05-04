using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;

[ProtoContract]
public class LegalPerson
{
    [ProtoMember(1)]
    public string Name { get; set; } = string.Empty;
}