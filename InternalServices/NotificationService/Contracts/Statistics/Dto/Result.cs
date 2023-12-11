using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Result
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }

    [ProtoMember(2)]
    public NotificationState State { get; set; }

    [ProtoMember(3)]
    public NotificationChannel Channel { get; set; }

    [ProtoMember(4)]
    public DateTime? RequestTimestamp { get; set; }

    [ProtoMember(5)]
    public SenderType Mandant { get; set; }
}
