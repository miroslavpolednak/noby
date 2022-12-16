using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest.Incomes;

internal class IncomeEmployment : IncomeBase
{
    private readonly Income _income;

    public IncomeEmployment(IncomeInList customerIncome, Income income) : base(customerIncome)
    {
        _income = income;
    }

    public required int FirstEmploymentTypeId { private get; init; }

    public int EmploymentTypeId => _income.Employement?.Job?.EmploymentTypeId ?? FirstEmploymentTypeId;

    public IncomeDataEmployement Employment => _income.Employement;

    public string? EmployerIdentificationNumber =>
        new[] { Employment?.Employer?.Cin, Employment?.Employer?.BirthNumber }.FirstOrDefault(str => !string.IsNullOrEmpty(str));

    public int DocumentType => 1;
}