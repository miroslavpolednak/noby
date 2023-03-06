namespace DomainServices.HouseholdService.Contracts;

public interface IIncome
{
    public int IncomeTypeId { get; set; }
    public IncomeBaseData BaseData { get; set; }
    public IncomeDataEmployement Employement { get; set; }
}
