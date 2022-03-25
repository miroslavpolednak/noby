using Google.Protobuf.Collections;
using System.Linq.Expressions;
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

    public static CustomerInHousehold? ToApiResponse(this contracts.CustomerOnSA model)
        => new CustomerInHousehold()
        {
            CustomerOnSAId = model.CustomerOnSAId,
            Identities = null,
            FirstName = model.FirstNameNaturalPerson,
            LastName = model.Name,
            DateOfBirth = model.DateOfBirthNaturalPerson,
            RoleId = model.CustomerRoleId,
            Incomes = model.Incomes is null ? null : model.Incomes.Select(x => new CustomerIncome.Dto.IncomeBaseData
            {
                Sum = x.Sum,
                CurrencyCode = x.CurrencyCode,
                IncomeId = x.IncomeId,
                IncomeTypeId = (CIS.Foms.Enums.CustomerIncomeTypes)x.IncomeTypeId
            }).ToList(),
            Obligations = model.Obligations is null ? null : model.Obligations.Select(x => new Dto.CustomerObligation
            {
                CreditCardLimit = x.CreditCardLimit,
                LoanPaymentAmount = x.LoanPaymentAmount,
                ObligationCreditor = (CIS.Foms.Enums.Mandants)(int)x.ObligationCreditor,
                ObligationTypeId = x.ObligationTypeId,
                RemainingLoanPrincipal = x.RemainingLoanPrincipal
            }).ToList()
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
