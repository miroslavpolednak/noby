namespace NOBY.Api.Endpoints.SalesArrangement.GetCreditWorthiness;

internal record GetCreditWorthinessRequest(int SalesArrangementId)
    : IRequest<GetCreditWorthinessResponse>
{
}
