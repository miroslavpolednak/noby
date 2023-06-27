using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

internal class LoanApplicationHousehold
{
    public required Household Household { get; init; }

    public required IList<LoanApplicationCustomer> Customers { get; init; }
}