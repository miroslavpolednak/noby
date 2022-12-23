using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Common;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class Identifier
{
    [ProtoMember(1)]
    public string Identity { get; set; } = null!;
    
    [ProtoMember(2)]
    public string IdentityScheme { get; set; } = null!;
}