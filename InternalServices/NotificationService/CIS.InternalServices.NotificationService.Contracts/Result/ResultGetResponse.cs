using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class ResultGetResponse
{
    [ProtoMember(1)]
    public string NotificationId { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public NotificationState State { get; set; }
    
    [ProtoMember(3)]
    public NotificationChannel Channel { get; set; }
    
    [ProtoMember(4)]
    public List<ResultError> Errors { get; set; } = new();
}