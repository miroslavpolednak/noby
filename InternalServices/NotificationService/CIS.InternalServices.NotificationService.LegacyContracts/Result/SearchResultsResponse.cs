using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result;

[ProtoContract]
public class SearchResultsResponse
{
    [ProtoMember(1)]
    public List<Dto.Result> Results { get; set; } = new();
}