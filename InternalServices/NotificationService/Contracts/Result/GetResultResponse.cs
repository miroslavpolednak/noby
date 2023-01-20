using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract]
public class GetResultResponse
{
    [ProtoMember(1)]
    public Dto.Result Result { get; set; } = null!;
}