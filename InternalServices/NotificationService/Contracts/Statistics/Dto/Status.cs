using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Status
{
    [ProtoMember(1)]
    public int? DELIVERED { get; set; }

    [ProtoMember(2)]
    public int? ERROR { get; set; }

    [ProtoMember(3)]
    public int? INPROGRESS { get; set; }

    [ProtoMember(4)]
    public int? SENT { get; set; }

    [ProtoMember(5)]
    public int? UNSENT { get; set; }

    [ProtoMember(6)]
    public int? INVALID { get; set; }
}