namespace FOMS.Api.Endpoints.CustomerIncome.GetIncomes;

internal record GetIncomesRequest(int CustomerOnSAId)
    : IRequest<List<IncomeInList>>
{
}
