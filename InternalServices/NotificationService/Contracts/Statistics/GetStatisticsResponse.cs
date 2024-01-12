using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics;

[ProtoContract]
public class GetStatisticsResponse
{
    [ProtoMember(1)]
    public required Dto.Statistics Statistics { get; set; }
}
