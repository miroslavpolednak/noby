using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Common;

[ProtoContract]
public class Identifier
{
    [ProtoMember(1)]
    public string Identity { get; set; } = null!;
    
    [ProtoMember(2)]
    public string IdentityScheme { get; set; } = null!;
}