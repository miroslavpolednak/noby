using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Statistics;

[ProtoContract]
public class GetDetailedStatisticsResponse
{
    [ProtoMember(1)]
    public required Dto.Statistics Statistics { get; set; }

    [ProtoMember(2)]
    public List<Dto.Result>? Results { get; set; }
}
