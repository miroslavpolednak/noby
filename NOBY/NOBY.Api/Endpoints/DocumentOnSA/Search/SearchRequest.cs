using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchRequest : IRequest<SearchResponse>
{
    [JsonIgnore]
    public int SalesArrangementId { get; set; }

    public int? EACodeMainId { get; set; }

    internal SearchRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
