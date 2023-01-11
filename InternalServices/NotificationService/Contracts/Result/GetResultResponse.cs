using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract]
public class GetResultResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
    
    [ProtoMember(2)]
    public NotificationState State { get; set; }
    
    [ProtoMember(3)]
    public NotificationChannel Channel { get; set; }
    
    [ProtoMember(4)]
    public List<string> Errors { get; set; } = new();
}