using __Contracts = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Household.UpdateHousehold;

internal static class Extensions
{
    public static __Contracts.HouseholdData? ToDomainServiceRequest(this Dto.HouseholdData? model)
        => model is null ? null : new __Contracts.HouseholdData()
        {
            ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
            ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount,
            PropertySettlementId = model.PropertySettlementId,
            AreBothPartnersDeptors = model.AreBothPartnersDeptors
        };

    public static __Contracts.Expenses? ToDomainServiceRequest(this Dto.HouseholdExpenses? model)
        => model is null ? null : new __Contracts.Expenses()
        {
            HousingExpenseAmount = model.HousingExpenseAmount,
            InsuranceExpenseAmount = model.InsuranceExpenseAmount,
            SavingExpenseAmount = model.SavingExpenseAmount,
            OtherExpenseAmount = model.OtherExpenseAmount
        };
}
