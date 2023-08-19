using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateDeedOfOwnershipDocument;

/// <summary>
/// RealEstateIds, které uživatel vybral k Ocenění
/// </summary>
public sealed class UpdateDeedOfOwnershipDocumentRequest
    : IRequest
{
    [JsonIgnore]
    internal long CaseId;

    [JsonIgnore]
    internal int RealEstateValuationId;

    [JsonIgnore]
    internal int DeedOfOwnershipDocumentId;

    /// <summary>
    /// Unikátní ID nemovitosti ze systému CREM
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public List<long> RealEstateIds { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    internal UpdateDeedOfOwnershipDocumentRequest InfuseId(long caseId, int realEstateValuationId, int deedOfOwnershipDocumentId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        DeedOfOwnershipDocumentId = deedOfOwnershipDocumentId;

        return this;
    }
}
