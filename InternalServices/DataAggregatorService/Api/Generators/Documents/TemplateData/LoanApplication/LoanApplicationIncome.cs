using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.LoanApplication;

public class LoanApplicationIncome
{
    private readonly ILookup<EnumIncomeTypes, IncomeInList> _incomesByType;

    public LoanApplicationIncome(CustomerOnSA customerOnSa)
    {
        _incomesByType = customerOnSa.Incomes.ToLookup(i => (EnumIncomeTypes)i.IncomeTypeId);
    }

    public decimal? IncomeEmployment => IncomeSum(EnumIncomeTypes.Employement);

    public decimal? IncomeEnterprise => IncomeSum(EnumIncomeTypes.Entrepreneur);

    public decimal? IncomeRent => IncomeSum(EnumIncomeTypes.Rent);

    public decimal? IncomeOther => IncomeSum(EnumIncomeTypes.Other);

    private decimal? IncomeSum(EnumIncomeTypes incomeType)
    {
        var result = _incomesByType[incomeType].Select(i => (decimal?)i.Sum).Where(income => income.HasValue).Sum();

        return result == 0m ? null : result;
    }
}