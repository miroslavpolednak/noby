using CIS.Foms.Enums;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

public class LoanApplicationIncome
{
    private readonly ILookup<CustomerIncomeTypes, IncomeInList> _incomesByType;

    public LoanApplicationIncome(CustomerOnSA customerOnSa)
    {
        _incomesByType = customerOnSa.Incomes.ToLookup(i => (CustomerIncomeTypes)i.IncomeTypeId);
    }

    public decimal? IncomeEmployment => IncomeSum(CustomerIncomeTypes.Employement);

    public decimal? IncomeEnterprise => IncomeSum(CustomerIncomeTypes.Enterprise);

    public decimal? IncomeRent => IncomeSum(CustomerIncomeTypes.Rent);

    public decimal? IncomeOther => IncomeSum(CustomerIncomeTypes.Other);

    private decimal? IncomeSum(CustomerIncomeTypes incomeType)
    {
        var result = _incomesByType[incomeType].Select(i => (decimal?)i.Sum).Where(income => income.HasValue).Sum();

        return result == 0m ? null : result;
    }
}