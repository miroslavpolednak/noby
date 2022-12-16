using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest.Incomes;

internal class IncomeEntrepreneur : IncomeBase
{
    private readonly Income _income;

    public IncomeEntrepreneur(IncomeInList customerIncome, Income income) : base(customerIncome)
    {
        _income = income;
    }

    public IncomeDataEntrepreneur? Entrepreneur => _income.Entrepreneur;

    public string? IdentificationNumber =>
        new[] { Entrepreneur?.Cin, Entrepreneur?.BirthNumber }.FirstOrDefault(i => !string.IsNullOrEmpty(i));

    public int DocumentType => 2;
}