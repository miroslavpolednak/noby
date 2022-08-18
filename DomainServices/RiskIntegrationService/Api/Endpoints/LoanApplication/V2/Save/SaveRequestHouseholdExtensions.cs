using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestHouseholdExtensions
{
    public static async Task<List<LoanApplicationHousehold>> ToC4m(this List<_V2.LoanApplicationHousehold> households, _RAT.RiskApplicationTypeItem riskApplicationType, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
    {
        var householdTypes = await _codebookService.HouseholdTypes(cancellation);
        var propSettlements = await _codebookService.PropertySettlements(cancellation);

        return households.Select(household => new LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = householdTypes.FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                HouseholdExpensesSummary = household.Expenses?.ToC4m(),
                SettlementTypeCode = propSettlements.FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                CounterParty = household.Customers?.ToC4m(riskApplicationType)
        })
            .ToList();
    }

    public static List<LoanApplicationCounterParty> ToC4m(this List<_V2.LoanApplicationCustomer> customers, _RAT.RiskApplicationTypeItem riskApplicationType)
        => customers.Select(customer => new LoanApplicationCounterParty
        {
            Id = customer.InternalCustomerId,
            CustomerId = new ResourceIdentifier
            {
                Id = customer.PrimaryCustomerId,
                Instance = Helpers.GetResourceInstanceFromMandant(riskApplicationType.MandantId),
                Domain = "CM",
                Resource = "Customer"
            },
            GroupEmployee = customer.IsGroupEmployee,

        })
        .ToList();

    public static List<ExpensesSummary> ToC4m(this Contracts.Shared.ExpensesSummary.V1.ExpensesSummary expenses)
        => new List<ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.ToAmount(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.ToAmount(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = expenses.Insurance.ToAmount(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.ToAmount(), Category = ExpensesSummaryCategory.OTHER }
        };
}
