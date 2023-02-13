using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Common;

[ProtoContract]
public class Error
{
    [ProtoMember(1)]
    public string Code { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string Message { get; set; } = string.Empty;
}