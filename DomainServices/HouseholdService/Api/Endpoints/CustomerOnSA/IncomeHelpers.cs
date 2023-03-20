namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA;

internal static class IncomeHelpers
{
    public static bool AlreadyHasMaxIncomes(CustomerIncomeTypes incomeType, int count)
        => incomeType switch
        {
            CustomerIncomeTypes.Employement => count >= 3,
            CustomerIncomeTypes.Enterprise => count >= 1,
            CustomerIncomeTypes.Rent => count >= 1,
            CustomerIncomeTypes.Other => count >= 10,
            _ => throw new NotImplementedException("This customer income type count check is not implemented")
        };
}