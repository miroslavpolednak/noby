using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetDetail;

internal sealed record GetDetailRequest(int SalesArrangementId)
    : IRequest<GetDetailResponse>
{
}