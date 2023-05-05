namespace NOBY.Api.Endpoints.CustomerIncome.DeleteIncome;

internal sealed record DeleteIncomeRequest(int CustomerOnSAId, int IncomeId)
    : IRequest
{
}
