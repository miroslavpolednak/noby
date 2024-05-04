using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

[ProtoContract]
public class SmsData
{
    [ProtoMember(1)]
    public Phone Phone { get; set; } = default!;
    
    [ProtoMember(2)]
    public string Type { get; set; } = string.Empty;
}