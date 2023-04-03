using __Contracts = DomainServices.HouseholdService.Contracts;
using NOBY.Api.Endpoints.CustomerObligation;

namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal static class Extensions
{
    public static GetHouseholdResponse ToApiResponse(this __Contracts.Household household)
        => new GetHouseholdResponse
        {
            Data = household.Data?.mapData(),
            Expenses = household.Expenses?.mapExpenses(),
            HouseholdId = household.HouseholdId
        };

    public static CustomerInHousehold? ToApiResponse(this __Contracts.CustomerOnSA model)
        => new CustomerInHousehold()
        {
            CustomerOnSAId = model.CustomerOnSAId,
            Identities = model.CustomerIdentifiers?.Select(t => new CIS.Foms.Types.CustomerIdentity(t.IdentityId, (int)t.IdentityScheme)).ToList(),
            FirstName = model.FirstNameNaturalPerson,
            LastName = model.Name,
            DateOfBirth = model.DateOfBirthNaturalPerson,
            RoleId = model.CustomerRoleId,
            MaritalStatusId = model.MaritalStatusId,
            LockedIncomeDateTime = model.LockedIncomeDateTime,
            Incomes = model.Incomes is null ? null : model.Incomes.Select(x => new CustomerIncome.Dto.IncomeBaseData
            {
                Sum = x.Sum,
                CurrencyCode = x.CurrencyCode,
                IncomeId = x.IncomeId,
                IncomeSource = x.IncomeSource,
                HasProofOfIncome = x.HasProofOfIncome,
                IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)x.IncomeTypeId
            }).ToList(),
            Obligations = model.Obligations is null ? null : model.Obligations.Select(x => x.ToApiResponse()).ToList()
        };

    static Dto.HouseholdExpenses? mapExpenses(this __Contracts.Expenses model)
        => new Dto.HouseholdExpenses()
            {
                InsuranceExpenseAmount = model.InsuranceExpenseAmount,
                SavingExpenseAmount = model.SavingExpenseAmount,
                HousingExpenseAmount = model.HousingExpenseAmount,
                OtherExpenseAmount = model.OtherExpenseAmount
            };

    static Dto.HouseholdData? mapData(this __Contracts.HouseholdData model)
        => new Dto.HouseholdData()
            {
                AreBothPartnersDeptors = model.AreBothPartnersDeptors,
                PropertySettlementId = model.PropertySettlementId,
                ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount
            };
}
