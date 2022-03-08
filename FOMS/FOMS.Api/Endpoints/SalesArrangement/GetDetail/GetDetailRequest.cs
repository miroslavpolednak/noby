using CIS.Core.Validation;

namespace FOMS.Api.Endpoints.SalesArrangement.GetDetail;

internal record GetDetailRequest(int SalesArrangementId)
    : IRequest<GetDetailResponse>
{
}