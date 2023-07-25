using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.AddDeedOfOwnershipDocument;

/// <summary>
/// Identifikační údaje nemovitosti k Ocenění (bez Noby ID)
/// </summary>
public sealed class AddDeedOfOwnershipDocumentRequest
    : IRequest<int>
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    public Dto.RealEstateValuation.DeedOfOwnershipDocument? DeedOfOwnershipDocument { get; set; }

    internal AddDeedOfOwnershipDocumentRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;

        return this;
    }
}
