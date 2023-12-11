using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Statistics
{
    [ProtoMember(1)]
    public Email? Email { get; set; }

    [ProtoMember(2)]
    public Sms? SMS { get; set; }
}
