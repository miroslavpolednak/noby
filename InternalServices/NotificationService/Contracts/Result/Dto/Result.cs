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
    public List<ResultError> Errors { get; set; } = new();
    
    [ProtoMember(5)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(6)]
    public long? CaseId { get; set; }
    
    [ProtoMember(7)]
    public string? CustomId { get; set; }
    
    [ProtoMember(8)]
    public string? DocumentId { get; set; }
    
    [ProtoMember(9)]
    public DocumentHash? DocumentHash { get; set; }

    [ProtoMember(10)]
    public DateTime? RequestTimestamp { get; set; }

    [ProtoMember(11)]
    public RequestData RequestData { get; set; } = null!;
    
    [ProtoMember(12)]
    public DateTime? ResultTimestamp { get; set; }

    [ProtoMember(13)]
    public string CreatedBy { get; set; } = null!;
}