using CIS.Foms.Enums;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

internal class HouseholdDto
{
    private readonly DomainServices.HouseholdService.Contracts.Household _household;

    public HouseholdDto(DomainServices.HouseholdService.Contracts.Household household, int number)
    {
        Number = number;
        _household = household;
    }

    public int HouseholdId => _household.HouseholdId;

    public int Number { get; }

    public HouseholdTypes HouseholdType => (HouseholdTypes)_household.HouseholdTypeId;

    public int ChildrenUpToTenYearsCount => _household.Data.ChildrenUpToTenYearsCount ?? 0;

    public int ChildrenOverTenYearsCount => _household.Data.ChildrenOverTenYearsCount ?? 0;

    public int? SavingExpenseAmount => _household.Expenses.SavingExpenseAmount;

    public int? InsuranceExpenseAmount => _household.Expenses.InsuranceExpenseAmount;

    public int? HousingExpenseAmount => _household.Expenses.HousingExpenseAmount;

    public int? OtherExpenseAmount => _household.Expenses.OtherExpenseAmount;

    public int PropertySettlementId => _household.Data.PropertySettlementId ?? 0;

    public int? CustomerOnSaId1 => _household.CustomerOnSAId1;

    public int? CustomerOnSaId2 => _household.CustomerOnSAId2;
}