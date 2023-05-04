namespace NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;

internal sealed record ValidateSalesArrangementRequest(int SalesArrangementId)
    : IRequest<ValidateSalesArrangementResponse>
{
}
