namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal record DeleteIncomeRequest(int CustomerOnSAId, int IncomeId)
    : IRequest
{
}
