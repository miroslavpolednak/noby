using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData.ProductRequest.Incomes;

internal class IncomeBase
{
    protected readonly IncomeInList _customerIncome;

    public IncomeBase(IncomeInList customerIncome)
    {
        _customerIncome = customerIncome;
    }

    public required int Number { get; init; }

    public decimal? IncomeSum => _customerIncome.Sum;

    public string CurrencyCode => _customerIncome.CurrencyCode;
}