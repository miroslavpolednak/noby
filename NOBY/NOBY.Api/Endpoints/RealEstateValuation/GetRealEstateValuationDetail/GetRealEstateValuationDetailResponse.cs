using NOBY.Api.Endpoints.RealEstateValuation.Shared;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

public class GetRealEstateValuationDetailResponse
{
    public required RealEstateValuationListItem RealEstateValuationListItem { get; init; }

    public required RealEstateValuationDetail RealEstateValuationDetail { get; init; }
}