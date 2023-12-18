using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Status
{
    [ProtoMember(1)]
    public int? Delivered { get; set; }

    [ProtoMember(2)]
    public int? Error { get; set; }

    [ProtoMember(3)]
    public int? InProgress { get; set; }

    [ProtoMember(4)]
    public int? Sent { get; set; }

    [ProtoMember(5)]
    public int? Unsent { get; set; }

    [ProtoMember(6)]
    public int? Invalid { get; set; }
}