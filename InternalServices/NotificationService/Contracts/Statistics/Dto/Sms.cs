using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Sms
{
    [ProtoMember(1)]
    public required Status Status { get; set; }
}
