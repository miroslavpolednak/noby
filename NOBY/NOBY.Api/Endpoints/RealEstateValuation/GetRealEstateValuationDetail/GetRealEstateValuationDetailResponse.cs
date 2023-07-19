using NOBY.Dto.RealEstateValuation;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

public sealed class GetRealEstateValuationDetailResponse
{
    public required RealEstateValuationListItem RealEstateValuationListItem { get; init; }

    public required RealEstateValuationDetail RealEstateValuationDetail { get; init; }

    public List<NOBY.Dto.RealEstateValuation.DeedOfOwnershipDocument>? DeedOfOwnershipDocuments { get; set; }
}