using CIS.Core.Validation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal sealed record GetSalesArrangementRequest(int SalesArrangementId)
    : IRequest<GetSalesArrangementResponse>
{
}