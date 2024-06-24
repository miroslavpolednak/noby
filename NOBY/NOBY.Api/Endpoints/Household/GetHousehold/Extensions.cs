using __Contracts = DomainServices.HouseholdService.Contracts;
using NOBY.Api.Endpoints.CustomerObligation;

namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal static class Extensions
{
    public static GetHouseholdResponse ToApiResponse(this __Contracts.Household household)
        => new GetHouseholdResponse
        {
            HouseholdId = household.HouseholdId,
            SalesArrangementId = household.SalesArrangementId,
            CaseId = household.CaseId,
            Data = household.Data?.mapData(),
            Expenses = household.Expenses?.mapExpenses()
        };

    public static CustomerInHousehold? ToApiResponse(this __Contracts.CustomerOnSA model)
        => new CustomerInHousehold()
        {
            CustomerOnSAId = model.CustomerOnSAId,
            Identities = model.CustomerIdentifiers?.Select(t => new SharedTypes.Types.CustomerIdentity(t.IdentityId, (int)t.IdentityScheme)).ToList(),
            FirstName = model.FirstNameNaturalPerson,
            LastName = model.Name,
            DateOfBirth = model.DateOfBirthNaturalPerson,
            RoleId = model.CustomerRoleId,
            MaritalStatusId = model.MaritalStatusId,
            LockedIncome = model.LockedIncomeDateTime is not null,
            LockedIncomeDateTime = model.LockedIncomeDateTime,
            Incomes = model.Incomes is null ? null : model.Incomes.Select(x => new IncomeBaseData
            {
                Sum = x.Sum,
                CurrencyCode = x.CurrencyCode,
                IncomeId = x.IncomeId,
                IncomeSource = x.IncomeSource,
                HasProofOfIncome = x.HasProofOfIncome,
                IncomeTypeId = (SharedTypes.Enums.CustomerIncomeTypes)x.IncomeTypeId
            }).ToList(),
            Obligations = model.Obligations is null ? null : model.Obligations.Select(x => x.ToApiResponse()).ToList()
        };

    static SharedDto.HouseholdExpenses? mapExpenses(this __Contracts.Expenses model)
        => new SharedDto.HouseholdExpenses()
            {
                InsuranceExpenseAmount = model.InsuranceExpenseAmount,
                SavingExpenseAmount = model.SavingExpenseAmount,
                HousingExpenseAmount = model.HousingExpenseAmount,
                OtherExpenseAmount = model.OtherExpenseAmount
            };

    static SharedDto.HouseholdData? mapData(this __Contracts.HouseholdData model)
        => new SharedDto.HouseholdData()
            {
                AreBothPartnersDeptors = model.AreBothPartnersDeptors,
                PropertySettlementId = model.PropertySettlementId,
                ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount
            };
}
