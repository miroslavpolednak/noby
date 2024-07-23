namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomersOnSa;

internal sealed record GetCustomersOnSaRequest(int SalesArrangementId)
    : IRequest<List<SalesArrangementGetCustomersOnSaItem>>
{
}