using NOBY.Dto.RealEstateValuation;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

public sealed class GetRealEstateValuationDetailResponse
{
    public required RealEstateValuationListItem RealEstateValuationListItem { get; init; }

    public required RealEstateValuationDetail RealEstateValuationDetail { get; init; }

    public List<RealEstateValuationAttachment>? Attachments { get; set; }

    public List<DeedOfOwnershipDocumentWithId>? DeedOfOwnershipDocuments { get; set; }
}

