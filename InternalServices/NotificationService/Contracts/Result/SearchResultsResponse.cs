using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract]
public class SearchResultsResponse
{
    [ProtoMember(1)]
    public List<Dto.Result> Results { get; set; } = new();
}