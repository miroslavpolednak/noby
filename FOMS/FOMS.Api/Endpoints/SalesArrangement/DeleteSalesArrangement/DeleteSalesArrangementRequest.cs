namespace FOMS.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed record DeleteSalesArrangementRequest(int SalesArrangementId)
    : IRequest
{
}
