using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.Incomes;

internal class IncomeOther : IncomeBase
{
    private readonly Income _income;

    public IncomeOther(IncomeInList customerIncome, Income income) : base(customerIncome)
    {
        _income = income;
    }

    public int? IncomeOtherTypeId => _income.Other?.IncomeOtherTypeId;
}