using CIS.InternalServices.NotificationService.Contracts.Common;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto;

[ProtoContract]
public class SmsData
{
    [ProtoMember(1)]
    public Phone Phone { get; set; } = default!;
    
    [ProtoMember(2)]
    public string Text { get; set; } = string.Empty;
}