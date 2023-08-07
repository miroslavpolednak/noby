using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaRequest : IRequest<SearchDocumentsOnSaResponse>
{
    [JsonIgnore]
    public int SalesArrangementId { get; set; }

    public int? EACodeMainId { get; set; }

    internal SearchDocumentsOnSaRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
