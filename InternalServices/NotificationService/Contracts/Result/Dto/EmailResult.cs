using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.NotificationService.Contracts.Common;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto;

public class EmailResult : Abstraction.Result
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
    
    [ProtoMember(2)]
    public NotificationState State { get; set; }
    
    [ProtoMember(3)]
    public NotificationChannel Channel { get; set; }
    
    [ProtoMember(4)]
    public List<string> Errors { get; set; } = new();
    
    [ProtoMember(5)]
    public Identifier Identifier { get; set; } = default!;
    
    [ProtoMember(6)]
    public string CustomId { get; set; } = string.Empty;
    
    [ProtoMember(7)]
    public string DocumentId { get; set; } = string.Empty;

    [ProtoMember(8)]
    public GrpcDateTime RequestTimestamp { get; set; } = default!;
    
    [ProtoMember(9)]
    public GrpcDateTime HandoverToMcsTimestamp { get; set; } = default!;
}