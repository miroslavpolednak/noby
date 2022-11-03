namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal record GetIncomeRequest(int SalesArrangementId, int IncomeId)
    : IRequest<GetIncomeResponse>
{
}
