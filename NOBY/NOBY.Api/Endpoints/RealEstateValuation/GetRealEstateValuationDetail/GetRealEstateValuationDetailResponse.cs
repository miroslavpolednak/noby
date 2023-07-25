using NOBY.Dto.RealEstateValuation;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

public sealed class GetRealEstateValuationDetailResponse
{
    public required RealEstateValuationListItem RealEstateValuationListItem { get; init; }

    public required RealEstateValuationDetail RealEstateValuationDetail { get; init; }

    public List<RealEstateValuationAttachment>? Attachments { get; set; }

    public List<GetRealEstateValuationDetailResponseDeed>? DeedOfOwnershipDocuments { get; set; }
}

public sealed class GetRealEstateValuationDetailResponseDeed
{
    /// <summary>
    /// Noby ID daného záznamu.Určuje jednoznačnou kombinaci cremDeedOfOwnershipDocumentId a RealEstateValuationId (Noby Ocenění) pro případy simulování více možností žádostí s jednou nemovitostí.
    /// </summary>
    public int DeedOfOwnershipDocumentId { get; set; }

    /// <summary>
    /// Identifikační údaje nemovitosti k Ocenění(bez Noby ID)
    /// </summary>
    public DeedOfOwnershipDocument? DeedOfOwnershipDocument { get; set; }
}