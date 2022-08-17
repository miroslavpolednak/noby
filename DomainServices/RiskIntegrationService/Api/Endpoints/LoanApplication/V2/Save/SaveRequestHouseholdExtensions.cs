using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal static class SaveRequestHouseholdExtensions
{
    public static async Task<List<LoanApplicationHousehold>> ToC4m(this List<_V2.LoanApplicationHousehold> households, CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService, CancellationToken cancellation)
    {
        var convertedHouseholds = new List<LoanApplicationHousehold>();
        foreach (var household in households)
        {
            var roleCode = (await _codebookService.HouseholdTypes(cancellation)).FirstOrDefault(t => t.Id == household.HouseholdTypeId)?.RdmCode;

            convertedHouseholds.Add(new LoanApplicationHousehold
            {
                Id = household.HouseholdId,
                RoleCode = roleCode,
                ChildrenUnderAnd10 = household.ChildrenUpToTenYearsCount,
                ChildrenOver10 = household.ChildrenOverTenYearsCount,
                SettlementTypeCode = (await _codebookService.PropertySettlements(cancellation)).FirstOrDefault(t => t.Id == household.PropertySettlementId)?.Code,
                HouseholdExpensesSummary = null,
                CounterParty = null
            });
        }
        return convertedHouseholds;
    }

    /*public static List<ExpensesSummary> ToC4m(this Contracts.Shared.ExpensesSummary.V1.ExpensesSummary expenses)
        => new List<ExpensesSummary>()
        {
            new() { Amount = expenses.Rent.GetValueOrDefault(), Category = ExpensesSummaryCategory.RENT },
            new() { Amount = expenses.Saving.GetValueOrDefault(), Category = ExpensesSummaryCategory.SAVINGS },
            new() { Amount = expenses.Insurance.GetValueOrDefault(), Category = ExpensesSummaryCategory.INSURANCE },
            new() { Amount = expenses.Other.GetValueOrDefault(), Category = ExpensesSummaryCategory.OTHER },
            new() { Amount = 0, Category = _C4M.ExpensesSummaryCategory.ALIMONY },
        };*/
}
