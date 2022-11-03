using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

internal record GetDetailRequest(int SalesArrangementId)
    : IRequest<GetDetailResponse>
{
}