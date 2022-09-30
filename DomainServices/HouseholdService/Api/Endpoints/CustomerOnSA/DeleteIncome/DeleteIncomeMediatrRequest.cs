namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.DeleteIncome;

internal record DeleteIncomeMediatrRequest(int IncomeId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
