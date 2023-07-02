using NOBY.Api.Endpoints.RealEstateValuation.Shared;

namespace NOBY.Api.Endpoints.RealEstateValuation.GetRealEstateValuationDetail;

public record GetRealEstateValuationDetailRequest(long CaseId, int RealEstateValuationId) 
    : IRequest<RealEstateValuationDetail>
{
}