using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SearchResultsResponse
{
    [ProtoMember(1)]
    public List<Dto.Abstraction.Result> Results { get; set; } = new();
}