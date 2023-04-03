namespace NOBY.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal sealed record GetCreditWorthinessRequest(int SalesArrangementId)
    : IRequest<GetCreditWorthinessResponse>
{
}
