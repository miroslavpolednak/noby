using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Common;

[ProtoContract]
public class DocumentHash
{
    [ProtoMember(1)]
    public string Hash { get; set; } = null!;
    
    [ProtoMember(2)]
    public string HashAlgorithm { get; set; } = null!;
}