using contracts = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

internal static class Extensions
{
    public static contracts.HouseholdData? ToDomainServiceRequest(this Dto.HouseholdData? model)
        => model is null ? null : new contracts.HouseholdData()
        {
            ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount,
            PropertySettlementId = model.PropertySettlementId,
            AreBothPartnersDeptors = model.AreBothPartnersDeptors
        };

    public static contracts.Expenses? ToDomainServiceRequest(this Dto.HouseholdExpenses? model)
        => model is null ? null : new contracts.Expenses()
        {
            HousingExpenseAmount = model.HousingExpenseAmount,
            InsuranceExpenseAmount = model.InsuranceExpenseAmount,
            SavingExpenseAmount = model.SavingExpenseAmount,
            OtherExpenseAmount = model.OtherExpenseAmount
        };
}
