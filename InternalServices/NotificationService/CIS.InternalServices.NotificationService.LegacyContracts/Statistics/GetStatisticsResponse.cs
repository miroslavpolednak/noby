using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Statistics;

[ProtoContract]
public class GetStatisticsResponse
{
    [ProtoMember(1)]
    public required Dto.Statistics Statistics { get; set; }
}
