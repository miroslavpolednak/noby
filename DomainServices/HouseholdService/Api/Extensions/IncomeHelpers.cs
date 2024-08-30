namespace DomainServices.HouseholdService.Api.Extensions;

internal static class IncomeHelpers
{
    public static bool AlreadyHasMaxIncomes(EnumIncomeTypes incomeType, int count)
        => incomeType switch
        {
            EnumIncomeTypes.Employement => count >= 3,
            EnumIncomeTypes.Entrepreneur => count >= 1,
            EnumIncomeTypes.Rent => count >= 1,
            EnumIncomeTypes.Other => count >= 10,
            _ => throw new NotImplementedException("This customer income type count check is not implemented")
        };
}