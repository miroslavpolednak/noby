using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SmsBaseRequest
{
    [ProtoMember(1)]
    public Phone Phone { get; set; } = default!;
    
    [ProtoMember(2)]
    public int ProcessingPriority { get; set; } = 1;

    [ProtoMember(3)]
    public string Type { get; set; } = string.Empty;
}