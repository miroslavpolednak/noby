using ProtoBuf;

namespace DomainServices.CaseService.Api.SharedDto;

[ProtoContract]
internal sealed class CaseStateChangeRequestId
{
    [ProtoMember(1)]
    public int RequestId { get; set; }

    [ProtoMember(2)]
    public long CaseId { get; set; }

    [ProtoMember(3)]
    public DateTime CreatedTime { get; set; }
}
