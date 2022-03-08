using contracts = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Household.GetHousehold;

internal static class Extensions
{
    public static GetHouseholdResponse ToApiResponse(this contracts.Household household)
        => new GetHouseholdResponse
        {
            Data = household.Data?.mapData(),
            Expenses = household.Expenses?.mapExpenses(),
            HouseholdId = household.HouseholdId
        };

    public static Dto.CustomerInHousehold? ToApiResponse(this contracts.CustomerOnSA model)
        => new Dto.CustomerInHousehold()
            {
                CustomerOnSAId = model.CustomerOnSAId,
                Identities = null,
                FirstName = model.FirstNameNaturalPerson,
                LastName = model.Name,
                DateOfBirth = model.DateOfBirthNaturalPerson,
                RoleId = model.CustomerRoleId
            };

    static Dto.HouseholdExpenses? mapExpenses(this contracts.Expenses model)
        => new Dto.HouseholdExpenses()
            {
                InsuranceExpenseAmount = model.InsuranceExpenseAmount,
                SavingExpenseAmount = model.SavingExpenseAmount,
                HousingExpenseAmount = model.HousingExpenseAmount,
                OtherExpenseAmount = model.OtherExpenseAmount
            };

    static Dto.HouseholdData? mapData(this contracts.HouseholdData model)
        => new Dto.HouseholdData()
            {
                PropertySettlementId = model.PropertySettlementId,
                ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount
            };
}
