using CIS.InternalServices.NotificationService.Contracts.Common;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto;

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
    public List<Error> Errors { get; set; } = new();
    
    [ProtoMember(5)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(6)]
    public string? CustomId { get; set; }
    
    [ProtoMember(7)]
    public string? DocumentId { get; set; }

    [ProtoMember(8)]
    public DateTime? RequestTimestamp { get; set; }

    [ProtoMember(9)]
    public RequestData RequestData { get; set; } = null!;
    
    [ProtoMember(10)]
    public DateTime? HandoverToMcsTimestamp { get; set; }

    [ProtoMember(11)]
    public string CreatedBy { get; set; } = null!;
}