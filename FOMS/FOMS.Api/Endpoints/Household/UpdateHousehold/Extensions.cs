using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.UpdateHousehold;

internal static class Extensions
{
    public static contracts.HouseholdData? ToDomainServiceRequest(this Dto.HouseholdData? model)
    {
        if (model is null) return null;
        return new contracts.HouseholdData()
        {
            ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount,
            PropertySettlementId = model.PropertySettlementId
        };
    }

    public static contracts.Expenses? ToDomainServiceRequest(this Dto.HouseholdExpenses? model)
    {
        if (model is null) return null;
        return new contracts.Expenses()
        {
            HousingExpenseAmount = model.HousingExpenseAmount,
            InsuranceExpenseAmount = model.InsuranceExpenseAmount,
            SavingExpenseAmount = model.SavingExpenseAmount,
            OtherExpenseAmount = model.OtherExpenseAmount
        };
    }
}
