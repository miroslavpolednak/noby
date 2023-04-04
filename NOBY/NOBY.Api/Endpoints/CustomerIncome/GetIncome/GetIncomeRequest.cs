namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal sealed record GetIncomeRequest(int SalesArrangementId, int IncomeId)
    : IRequest<GetIncomeResponse>
{
}
