using __Contracts = DomainServices.HouseholdService.Contracts;
using NOBY.Api.Endpoints.CustomerObligation;

namespace NOBY.Api.Endpoints.Household.GetHousehold;

internal static class Extensions
{
    public static HouseholdGetHouseholdResponse ToApiResponse(this __Contracts.Household household)
        => new()
        {
            HouseholdId = household.HouseholdId,
            SalesArrangementId = household.SalesArrangementId,
            CaseId = household.CaseId,
            Data = household.Data?.mapData(),
            Expenses = household.Expenses?.mapExpenses()
        };

    public static HouseholdCustomerInHousehold? ToApiResponse(this __Contracts.CustomerOnSA model)
        => new()
        {
            CustomerOnSAId = model.CustomerOnSAId,
            Identities = model.CustomerIdentifiers?.Select(t => (SharedTypesCustomerIdentity)t!).ToList(),
            FirstName = model.FirstNameNaturalPerson,
            LastName = model.Name,
            DateOfBirth = model.DateOfBirthNaturalPerson,
            RoleId = model.CustomerRoleId,
            MaritalStatusId = model.MaritalStatusId,
            LockedIncome = model.LockedIncomeDateTime is not null,
            LockedIncomeDateTime = model.LockedIncomeDateTime,
            IsIdentificationRequested = !(model.CustomerIdentifiers?.Any() ?? false),
            Incomes = model.Incomes is null ? null : model.Incomes.Select(x => new HouseholdIncomeBaseData
            {
                Sum = x.Sum,
                CurrencyCode = x.CurrencyCode,
                IncomeId = x.IncomeId,
                IncomeSource = x.IncomeSource,
                HasProofOfIncome = x.HasProofOfIncome,
                IncomeTypeId = (SharedTypes.Enums.EnumIncomeTypes)x.IncomeTypeId
            }).ToList(),
            Obligations = model.Obligations?.Select(x => x.ToApiResponse()).ToList()
        };

    static HouseholdExpenses? mapExpenses(this __Contracts.Expenses model)
        => new()
            {
                InsuranceExpenseAmount = model.InsuranceExpenseAmount,
                SavingExpenseAmount = model.SavingExpenseAmount,
                HousingExpenseAmount = model.HousingExpenseAmount,
                OtherExpenseAmount = model.OtherExpenseAmount
            };

    static HouseholdData? mapData(this __Contracts.HouseholdData model)
        => new()
            {
                AreBothPartnersDeptors = model.AreBothPartnersDeptors,
                PropertySettlementId = model.PropertySettlementId,
                ChildrenOverTenYearsCount = model.ChildrenOverTenYearsCount,
                ChildrenUpToTenYearsCount = model.ChildrenUpToTenYearsCount
            };
}
