using System.Linq.Expressions;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Repositories;

internal static class HouseholdExpressions
{
    public static Expression<Func<Entities.Household, Contracts.Household>> HouseholdDetail()
    {
        return t => new Contracts.Household()
        {
            HouseholdId = t.HouseholdId,
            SalesArrangementId = t.SalesArrangementId,
            CaseId = t.CaseId,
            HouseholdTypeId = (int)t.HouseholdTypeId,
            CustomerOnSAId1 = t.CustomerOnSAId1,
            CustomerOnSAId2 = t.CustomerOnSAId2,
            Data = new HouseholdData
            {
                ChildrenOverTenYearsCount = t.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = t.ChildrenUpToTenYearsCount,
                PropertySettlementId = t.PropertySettlementId,
                AreBothPartnersDeptors = t.AreBothPartnersDeptors
            },
            Expenses = new Expenses
            {
                HousingExpenseAmount = t.HousingExpenseAmount,
                SavingExpenseAmount = t.SavingExpenseAmount,
                OtherExpenseAmount = t.OtherExpenseAmount,
                InsuranceExpenseAmount = t.InsuranceExpenseAmount
            }
        };
    }
}