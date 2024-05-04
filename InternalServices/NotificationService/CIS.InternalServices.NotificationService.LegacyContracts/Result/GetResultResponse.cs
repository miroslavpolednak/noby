using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result;

[ProtoContract]
public class GetResultResponse
{
    [ProtoMember(1)]
    public Dto.Result Result { get; set; } = null!;
}