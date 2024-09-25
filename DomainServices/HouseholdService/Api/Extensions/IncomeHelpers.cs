using SharedComponents.DocumentDataStorage;

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

    public static bool IsNotIncomeEmployerUnique(string? cin, string? birthNumber, List<DocumentDataItem<Database.DocumentDataEntities.Income, int>> employmentIncomes)
    {
        return employmentIncomes.Any(t =>
                (!string.IsNullOrEmpty(t.Data!.Employement?.Employer?.Cin) && t.Data!.Employement?.Employer?.Cin == cin)
                || (!string.IsNullOrEmpty(t.Data!.Employement?.Employer?.BirthNumber) && t.Data!.Employement?.Employer?.BirthNumber == birthNumber));
    }
}